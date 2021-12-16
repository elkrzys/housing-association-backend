using System;
using System.Collections.Generic;
using System.IO;
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
            => (await _unitOfWork.DocumentsRepository.FindByIdAsync(id)).AsDto() ?? throw new NotFoundException();
        
        public async Task<List<DocumentDto>> FindAll()
        {
            // TODO: add refresh method to remove all files that are older than their expiration date
            var documents = await _unitOfWork.DocumentsRepository.FindAllAsync();
            return DocumentsToDocumentsDtos(documents);
        }

        public async Task<List<DocumentDto>> FindAllByAuthorId(int authorId)
        {
            var documents = await _unitOfWork.DocumentsRepository.FindAllByAuthorIdAsync(authorId) ??
                            throw new NotFoundException();
            
            return DocumentsToDocumentsDtos(documents);
        }

        public async Task AddNewDocument(UploadDocumentRequest request)
        {
            CheckIfDocumentFileIsValidOrThrowBadRequest(request.DocumentFile);
            var documentMd5 = await GetMd5IfDocumentNotExists(request.DocumentFile);
            var documentReceivers = await GetReceiversByIds(request.ReceiversIds);
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
            _unitOfWork.Commit();
        }

        private void CheckIfDocumentFileIsValidOrThrowBadRequest(IFormFile documentFile)
        {
            // TODO: check if document is pdf
            if (documentFile is null || documentFile.Length == 0)
                throw new BadRequestException("Request must include a valid document");

            var extension = Path.GetExtension(documentFile.FileName);
            
            if (string.IsNullOrEmpty(extension) || extension != ".pdf")
                throw new BadRequestException("Request must include a valid document");
        }
        
        private List<DocumentDto> DocumentsToDocumentsDtos(List<Document> documents)
        {
            List<DocumentDto> documentsDtos = new();
            documents.ForEach(document => documentsDtos.Add(document.AsDto()));
            return documentsDtos;
        }

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
                        throw new BadRequestException("File already exists");
                });
            }
            return currentDocHash;
        }

        private async Task<List<User>> GetReceiversByIds(List<int> ids)
        {
            List<User> receivers = new();
            ids.ForEach(async id => receivers.Add(await _unitOfWork.UsersRepository.FindByIdAsync(id)));
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
            _unitOfWork.Commit();
            
            if (File.Exists(path))
                File.Delete(path);
        }

    }
}