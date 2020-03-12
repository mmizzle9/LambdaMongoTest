using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1.Mongo
{
	public class MongoLayer : IMongoLayer
	{
		private readonly IMongoDatabase db;

		public MongoLayer(IMongoDatabase database)
		{
			db = database;
		}

		public void InsertDocument<T>(string collectionName, T document)
		{
			var collection = db.GetCollection<T>(collectionName);
			collection.InsertOne(document);
		}

		public List<T> LoadAllDocuments<T>(string collectionName)
		{
			var collection = db.GetCollection<T>(collectionName);

			return collection.Find(new BsonDocument()).ToList();
		}

		public T LoadDocumentById<T>(string collectionName, Guid id)
		{
			var collection = db.GetCollection<T>(collectionName);
			var filter = Builders<T>.Filter.Eq("Id", id);

			return collection.Find(filter).First();
		}

		public void UpdateDocument<T>(string collectionName, Guid id, T document)
		{
			var collection = db.GetCollection<T>(collectionName);

			collection.ReplaceOne(
				new BsonDocument("_id", id),
				document,
				new ReplaceOptions { IsUpsert = false });
		}

		public void DeleteDocument<T>(string collectionName, Guid id)
		{
			var collection = db.GetCollection<T>(collectionName);
			var filter = Builders<T>.Filter.Eq("Id", id);
			collection.DeleteOne(filter);

		}
	}
}
