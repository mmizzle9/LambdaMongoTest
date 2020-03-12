using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FunctionApp1.Mongo
{
	public interface IMongoLayer
	{
		void InsertDocument<T>(string collectionName, T document);

		List<T> LoadAllDocuments<T>(string collectionName);

		T LoadDocumentById<T>(string collectionName, Guid id);

		void UpdateDocument<T>(string collectionName, Guid id, T document);

		void DeleteDocument<T>(string collectionName, Guid id);
	}
}
