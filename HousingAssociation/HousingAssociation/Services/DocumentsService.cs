using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Controllers.Requests;
using HousingAssociation.DataAccess;
using HousingAssociation.DataAccess.Entities;
using HousingAssociation.ExceptionHandling.Exceptions;
using HousingAssociation.Models.DTOs;
using HousingAssociation.Utils;
using HousingAssociation.Utils.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace HousingAssociation.Services
{
    public class DocumentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentsService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<DocumentDto> FindById(int id)
        {
            var document = await _unitOfWork.DocumentsRepository.FindByIdAsync(id);
            if (document is null)
            {
                Log.Warning($"Document with id = {id} doesn't exist.");
                throw new NotFoundException();
            }
            return document.AsDto();
        }

        public async Task<List<DocumentDto>> FindAll()
        {
            var documents = await _unitOfWork.DocumentsRepository.FindAllAsync();
            documents = await DeleteExpiredDocuments(documents);
            return DocumentsToDocumentsDtos(documents);
        }

        public async Task<List<DocumentDto>> FindAllByAuthorId(int authorId)
        {
            var author = await _unitOfWork.UsersRepository.FindByIdAsync(authorId);
            if (author is null)
            {
                Log.Warning($"User with id = {authorId} doesn't exist.");
                throw new NotFoundException();
            }

            var documents = await _unitOfWork.DocumentsRepository.FindAllByAuthorIdAsync(authorId);
            return DocumentsToDocumentsDtos(documents);
        }

        public async Task<List<DocumentDto>> FindAllByReceiverId(int receiverId)
        {
            var receiver = await _unitOfWork.UsersRepository.FindByIdAsync(receiverId);
            if (receiver is null)
            {
                Log.Warning($"User with id = {receiverId} doesn't exist.");
                throw new NotFoundException();
            }
            var documents = await _unitOfWork.DocumentsRepository.FindAllByReceiverAsync(receiverId);
            return DocumentsToDocumentsDtos(documents);
        }

        public async Task AddNewDocument(UploadDocumentRequest request)
        {
            CheckIfDocumentFileIsValidOrThrowBadRequest(request.DocumentFile);
            var documentMd5 = await GetMd5IfDocumentNotExists(request.DocumentFile);
            var documentReceivers = await GetReceiversByIds(request.ReceiversIds);
            
            _unitOfWork.SetModified(documentReceivers);

            var path = await SaveFileAndReturnPath(request.DocumentFile);
            
            var document = new Document
            {
                Title = request.Title,
                AuthorId = request.AuthorId,
                Receivers = documentReceivers,
                Created = DateTime.Now,
                DaysToExpire = request.DaysToExpire,
                Md5 = documentMd5,
                Filepath = path
            };
            await _unitOfWork.DocumentsRepository.AddAsync(document);
            await _unitOfWork.CommitAsync();
        }

        private void CheckIfDocumentFileIsValidOrThrowBadRequest(IFormFile documentFile)
        {
            if (documentFile is null || documentFile.Length == 0)
            {
                Log.Warning($"Attempt to upload an empty file.");
                throw new BadRequestException("Request must include a valid document");
            }
            var extension = Path.GetExtension(documentFile.FileName);
            
            if (string.IsNullOrEmpty(extension) || extension != ".pdf")
            {
                Log.Warning($"Attempt to upload file with extension: {extension}");
                throw new BadRequestException("Request must include a valid document");
            }
        }
        
        private List<DocumentDto> DocumentsToDocumentsDtos(List<Document> documents) 
            => documents.Select(document => document.AsDto()).ToList();

        private async Task<string> GetMd5IfDocumentNotExists(IFormFile documentFile)
        {
            var existingMd5s = await _unitOfWork.DocumentsRepository.FindAllDocumentHashes();
            string currentDocHash = null;

            await using (var ms = new MemoryStream())
            {
                documentFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                
                existingMd5s.ForEach(hash =>
                {
                    currentDocHash = HashGenerator.CalculateMd5StringFromBytes(fileBytes);
                    if(hash.Equals(currentDocHash))
                    {
                        Log.Warning($"File with exact hash already exists.");
                        throw new BadRequestException("File already exists");
                    }
                });
            }
            return currentDocHash;
        }

        private async Task<List<User>> GetReceiversByIds(List<int> ids)
        {
            List<User> receivers = new();
            foreach (var id in ids)
            {
                receivers.Add(await _unitOfWork.UsersRepository.FindByIdAsync(id));
            }
            return receivers;
        }

        private async Task<string> SaveFileAndReturnPath(IFormFile documentFile)
        {
            var randomFileName = Path.GetRandomFileName();
            var extension = Path.GetExtension(documentFile.FileName);
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "documents", randomFileName + extension);
            
            await using var fileStream = new FileStream(path, FileMode.Create);
            await documentFile.CopyToAsync(fileStream);
            
            return path;
        }

        public async Task DeleteById(int id)
        {
            var document = await _unitOfWork.DocumentsRepository.FindByIdAsync(id) ?? throw new NotFoundException();
            var path = document.Filepath;
            
            _unitOfWork.DocumentsRepository.DeleteDocument(document);
            await _unitOfWork.CommitAsync();
            
            if (File.Exists(path))
                File.Delete(path);
        }

        private async Task<List<Document>> DeleteExpiredDocuments(List<Document> documents)
        {
            var expiredDocuments = documents.Where(d =>
                d.DaysToExpire != null && DateTimeOffset.Now > d.Created.AddDays(d.DaysToExpire!.Value)).ToList();
            documents.RemoveAll(d => expiredDocuments.Any(expired => expired.Id == d.Id));
            Log.Information($"Removed {expiredDocuments.Count} expired files");
            foreach (var expired in expiredDocuments)
            {
                await DeleteById(expired.Id);
            }
            return documents;
        }
    }
}