﻿using System;

namespace Raven.Client.Document
{
	/// <summary>
	/// Options for handling failover scenarios in replication environment
	/// </summary>
	[Flags]
	public enum FailoverBehavior
	{
		/// <summary>
		/// Allow to read from the secondary server(s), but immediately fail writes
		/// to the secondary server(s).
		/// </summary>
		/// <remarks>
		/// This is usually the safest approach, because it means that you can still serve
		/// read requests when the primary node is down, but don't have to deal with replication
		/// conflicts if there are writes to the secondary when the primary node is down.
		/// </remarks>
		AllowReadsFromSecondaries = 1,
		/// <summary>
		/// Allow to read from the secondary server(s), but immediately fail writes
		/// to the secondary server(s).
		/// </summary>
		/// <remarks>
		/// Choosing this option requires that you'll have some way of propogating changes
		/// made to the secondary server(s) to the primary node when the primary goes back
		/// up. 
		/// A typical strategy to handle this is to make sure that the replication is setup
		/// in a master/master relationship, so any writes to the secondary server will be 
		/// replicated to the master server.
		/// Please note, however, that this means that your code must be prepared to handle
		/// conflicts in case of different writes to the same document across nodes.
		/// </remarks>
		AllowReadsFromSecondariesAndWritesToSecondaries = 3,
		/// <summary>
		/// Immediately fail the request, without attempting any failover. This is true for both 
		/// reads and writes. The RavenDB client will not even check that you are using replication.
		/// </summary>
		/// <remarks>
		/// This is mostly useful when your replication setup is meant to be used for backups / external
		/// needs, and is not meant to be a failover storage.
		/// </remarks>
		FailImmediately = 0,

		/// <summary>
		/// Read requests will be spread across all the servers, instead of doing all the work against the master.
		/// This should give better read performance, overall. Write requests will always go to the master.
		/// </summary>
		ReadFromAllServers = 1024,
	}
}