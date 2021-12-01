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
using Microsoft.AspNetCore.Http;

namespace HousingAssociation.Services
{
    public class DocumentsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            CheckIfDocumentFileIsValidOrThrowBadRequest(request.Document);
            var documentMd5 = await GetMd5IfDocumentNotExists(request.Document);
            var documentReceivers = await GetReceiversByIds(request.ReceiversIds);

            // TODO: ADD FILEPATH
            var document = new Document
            {
                Title = request.Title,
                AuthorId = request.AuthorId,
                Receivers = documentReceivers,
                CreatedAt = DateTime.Now,
                DaysToExpire = request.DaysToExpire,
                Md5 = documentMd5,
                Filepath = "FILEPATH"
            };
            await _unitOfWork.DocumentsRepository.AddAsync(document);
            _unitOfWork.Commit();
            
            // TODO: save on server filesystem
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
            ids.ForEach(async id => receivers.Add(await _unitOfWork.UsersRepository.FindById(id)));
            return receivers;
        }
        
    }
}