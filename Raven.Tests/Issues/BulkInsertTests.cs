﻿using Raven.Abstractions.Data;
using Raven.Client.Connection;
using Raven.Client.Document;
using Raven.Json.Linq;
using Xunit;

namespace Raven.Tests.Issues
{
	public class BulkInsertTests : RavenTest
	{
		[Fact]
		public void CanCreateAndDisposeUsingBulk()
		{
			using(var store = NewRemoteDocumentStore())
			{
				var bulkInsertOperation = new RemoteBulkInsertOperation(new BulkInsertOptions(), (ServerClient) store.DatabaseCommands);
				bulkInsertOperation.Dispose();
			}
		}

		[Fact]
		public void CanHandleUpdates()
		{
			using (var store = NewRemoteDocumentStore())
			{
				using(var op = new RemoteBulkInsertOperation(new BulkInsertOptions(), (ServerClient)store.DatabaseCommands))
				{
					op.Write("items/1", new RavenJObject(), new RavenJObject());
				}

				using (var op = new RemoteBulkInsertOperation(new BulkInsertOptions
				{
					CheckForUpdates = true
				}, (ServerClient)store.DatabaseCommands))
				{
					op.Write("items/1", new RavenJObject(), new RavenJObject());
				}
			}
		}


		[Fact]
		public void CanHandleRefrenceChecking()
		{
			using (var store = NewRemoteDocumentStore())
			{
				using (var op = new RemoteBulkInsertOperation(new BulkInsertOptions
				{
					CheckReferencesInIndexes = true
				}, (ServerClient)store.DatabaseCommands))
				{
					op.Write("items/1", new RavenJObject(), new RavenJObject());
				}
			}
		}

		[Fact]
		public void CanInsertSingleDocument()
		{
			using (var store = NewRemoteDocumentStore())
			{
				var bulkInsertOperation = new RemoteBulkInsertOperation(new BulkInsertOptions(), (ServerClient)store.DatabaseCommands);
				bulkInsertOperation.Write("test", new RavenJObject(), new RavenJObject{{"test","passed"}});
				bulkInsertOperation.Dispose();

				Assert.Equal("passed", store.DatabaseCommands.Get("test").DataAsJson.Value<string>("test"));
			}
		}

		[Fact]
		public void CanInsertSeveralDocuments()
		{
			using (var store = NewRemoteDocumentStore())
			{
				var bulkInsertOperation = new RemoteBulkInsertOperation(new BulkInsertOptions(), (ServerClient)store.DatabaseCommands);
				bulkInsertOperation.Write("one", new RavenJObject(), new RavenJObject { { "test", "passed" } });
				bulkInsertOperation.Write("two", new RavenJObject(), new RavenJObject { { "test", "passed" } });
				bulkInsertOperation.Dispose();

				Assert.Equal("passed", store.DatabaseCommands.Get("one").DataAsJson.Value<string>("test"));
				Assert.Equal("passed", store.DatabaseCommands.Get("two").DataAsJson.Value<string>("test"));
			}
		}

		[Fact]
		public void CanInsertSeveralDocumentsInSeveralBatches()
		{
			using (var store = NewRemoteDocumentStore())
			{
				var bulkInsertOperation = new RemoteBulkInsertOperation(new BulkInsertOptions {BatchSize = 2}, (ServerClient) store.DatabaseCommands);
				bulkInsertOperation.Write("one", new RavenJObject(), new RavenJObject { { "test", "passed" } });
				bulkInsertOperation.Write("two", new RavenJObject(), new RavenJObject { { "test", "passed" } });
				bulkInsertOperation.Write("three", new RavenJObject(), new RavenJObject { { "test", "passed" } });
				bulkInsertOperation.Dispose();

				Assert.Equal("passed", store.DatabaseCommands.Get("one").DataAsJson.Value<string>("test"));
				Assert.Equal("passed", store.DatabaseCommands.Get("two").DataAsJson.Value<string>("test"));
				Assert.Equal("passed", store.DatabaseCommands.Get("three").DataAsJson.Value<string>("test"));
			}
		}
	}
}