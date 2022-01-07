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
        
        public async Task<List<DocumentDto>> FindAllSendByAssociation()
        {
            var documents = await _unitOfWork.DocumentsRepository.FindAllWhereReceiversNotEmpty();
            return DocumentsToDocumentsDtos(documents);
        }
        
        public async Task<List<DocumentDto>> FindAllSendByResidents()
        {
            var documents = await _unitOfWork.DocumentsRepository.FindAllWhereReceiversEmpty();
            return DocumentsToDocumentsDtos(documents);
        }

        public async Task AddNewDocument(UploadDocumentRequest request)
        {
            CheckIfDocumentFileIsValidOrThrowBadRequest(request.DocumentFile);
            var author = await _unitOfWork.UsersRepository.FindByIdAsync(request.AuthorId);
            if (author is null)
            {
                Log.Warning($"User with id = {request.AuthorId} not found.");
                throw new NotFoundException();
            }

            var documentMd5 = await GetMd5(request.DocumentFile);
            var existingDocument = await _unitOfWork.DocumentsRepository.FindByHashAsync(documentMd5);
            if (existingDocument is not null)
            {
                await UpdateReceivers(existingDocument, request.ReceiversIds);
                return;
            }
           
            List<User> documentReceivers = null;
            if (author.Role is not Role.Resident)
            {
                documentReceivers = await GetReceiversByIds(request.ReceiversIds);
                _unitOfWork.SetModified(documentReceivers);
            }

            var path = await SaveFileAndReturnFileName(request.DocumentFile);
            
            var document = new Document
            {
                Title = request.Title,
                AuthorId = request.AuthorId,
                Receivers = documentReceivers,
                Created = DateTime.Now,
                Removes = request.Removes,
                Md5 = documentMd5,
                Filepath = path
            };
            try
            {
                await _unitOfWork.DocumentsRepository.AddAsync(document);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                DeleteFromFileSystem(path);
            }
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

        private async Task<string> GetMd5(IFormFile documentFile)
        {
            string currentDocHash = null;
            using (var ms = new MemoryStream())
            {
                await documentFile.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                currentDocHash = HashGenerator.CalculateMd5StringFromBytes(fileBytes);
            }
            return currentDocHash;
        }

        private async Task UpdateReceivers(Document document, List<int> receiversIds)
        {
            var newReceivers = await GetReceiversByIds(receiversIds);
            var concatReceivers = document.Receivers.Concat(newReceivers).Distinct().ToList();
            _unitOfWork.SetModified(concatReceivers);
            document.Receivers = concatReceivers;
            await _unitOfWork.CommitAsync();
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

        private async Task<string> SaveFileAndReturnFileName(IFormFile documentFile)
        {
            var randomFileName = Path.GetRandomFileName();
            var extension = Path.GetExtension(documentFile.FileName);
            var fullFileName = randomFileName + extension;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "documents", fullFileName);
            await using var fileStream = new FileStream(path, FileMode.Create);
            await documentFile.CopyToAsync(fileStream);
            return fullFileName;
        }

        public async Task DeleteById(int id)
        {
            var document = await _unitOfWork.DocumentsRepository.FindByIdAsync(id);
            if(document is null)
            {
                Log.Warning($"Document with id = {id} not found.");
                throw new NotFoundException();
            }
            _unitOfWork.DocumentsRepository.DeleteDocument(document);
            await _unitOfWork.CommitAsync();
            DeleteFromFileSystem(document.Filepath);
        }

        private void DeleteFromFileSystem(string filename)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "documents", filename);
            if (File.Exists(path))
                File.Delete(path);
        }

        private async Task<List<Document>> DeleteExpiredDocuments(List<Document> documents)
        {
            var expiredDocuments = documents.Where(d =>
                d.Removes != null && DateTimeOffset.Now.Date >= d.Removes?.Date).ToList();
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