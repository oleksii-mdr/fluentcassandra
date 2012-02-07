﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Apache.Cassandra;
using FluentCassandra.Operations;
using FluentCassandra.Types;

namespace FluentCassandra
{
	public class CassandraKeyspace
	{
		private readonly string _keyspaceName;
		private CassandraKeyspaceSchema _cachedSchema;
		private CassandraContext _context;

		public CassandraKeyspace(string keyspaceName, CassandraContext context)
			: this(new CassandraKeyspaceSchema { Name = keyspaceName }, context) { }

		public CassandraKeyspace(CassandraKeyspaceSchema schema, CassandraContext context)
		{
			if (schema == null)
				throw new ArgumentNullException("schema");

			if (schema.Name == null)
				throw new ArgumentException("Must specify the keyspace name.");

			_keyspaceName = schema.Name;
			_cachedSchema = schema;
			_context = context;
		}

		/// <summary>
		/// 
		/// </summary>
		public string KeyspaceName
		{
			get { return _keyspaceName; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return KeyspaceName;
		}

		public void TryCreateSelf()
		{
			if (_context.KeyspaceExists(KeyspaceName))
			{
				Debug.WriteLine(KeyspaceName + " already exists", "keyspace");
				return;
			}

			string result = _context.AddKeyspace(new KsDef {
				Name = KeyspaceName,
				Strategy_class = "org.apache.cassandra.locator.NetworkTopologyStrategy",
				Replication_factor = 1,
				Cf_defs = new List<CfDef>(0)
			});
			Debug.WriteLine(result, "keyspace setup");
		}

		public void TryCreateColumnFamily(CassandraColumnFamilySchema schema)
		{
			try
			{
				var def = new CfDef {
					Keyspace = KeyspaceName,
					Name = schema.FamilyName,
					Comment = schema.FamilyDescription,
					Column_type = schema.FamilyType.ToString(),
					Key_alias = schema.KeyName.ToBigEndian(),
					Key_validation_class = GetCassandraComparatorType(schema.KeyType),
					Comparator_type = GetCassandraComparatorType(schema.ColumnNameType),
					Default_validation_class = GetCassandraComparatorType(schema.DefaultColumnValueType)
				};

				if (schema.FamilyType == ColumnType.Super)
				{
					def.Comparator_type = GetCassandraComparatorType(schema.SuperColumnNameType);
					def.Subcomparator_type = GetCassandraComparatorType(schema.ColumnNameType);
				}

				string result = _context.AddColumnFamily(def);
				Debug.WriteLine(result, "column family setup");
			}
			catch
			{
				Debug.WriteLine(schema.FamilyName + " already exists", "column family setup");
			}
		}

		[Obsolete("Use \"TryCreateColumnFamily\" class with out generic type")]
		public void TryCreateColumnFamily<CompareWith>(string columnFamilyName)
			where CompareWith : CassandraType
		{
			TryCreateColumnFamily(new CassandraColumnFamilySchema {
				ColumnNameType = typeof(CompareWith),
				FamilyName = columnFamilyName
			});
		}

		[Obsolete("Use \"TryCreateColumnFamily\" class with out generic type")]
		public void TryCreateColumnFamily<CompareWith, CompareSubcolumnWith>(string columnFamilyName)
			where CompareWith : CassandraType
			where CompareSubcolumnWith : CassandraType
		{
			TryCreateColumnFamily(new CassandraColumnFamilySchema(type: ColumnType.Super) {
				ColumnNameType = typeof(CompareWith),
				SuperColumnNameType = typeof(CompareSubcolumnWith),
				FamilyName = columnFamilyName
			});
		}

		private string GetCassandraComparatorType(Type comparatorType)
		{
			var comparatorTypeName = comparatorType.Name;

			// need to ignore generic dynamic composite types
			if (comparatorType.IsGenericType && comparatorTypeName.StartsWith("CompositeType"))
			{
				var compositeTypes = comparatorType.GetGenericArguments();
				var typeBuilder = new StringBuilder();

				typeBuilder.Append(comparatorTypeName.Substring(0, comparatorTypeName.IndexOf('`')));
				typeBuilder.Append("(");
				typeBuilder.Append(String.Join(",", compositeTypes.Select(t => t.Name)));
				typeBuilder.Append(")");

				comparatorTypeName = typeBuilder.ToString();
			}

			return comparatorTypeName;
		}

		public KsDef GetDescription()
		{
			return _context.ExecuteOperation(new SimpleOperation<Apache.Cassandra.KsDef>(ctx => {
				return ctx.Session.GetClient().describe_keyspace(KeyspaceName);
			}));
		}

		public CassandraColumnFamilySchema GetColumnFamilySchema(string columnFamily)
		{
			return GetSchema().ColumnFamilies.FirstOrDefault(cf => cf.FamilyName == columnFamily);
		}

		public bool ColumnFamilyExists(string columnFamilyName)
		{
			return GetSchema().ColumnFamilies.Any(cf => cf.FamilyName == columnFamilyName);
		}

		public void ClearCachedKeyspaceDescription()
		{
			_cachedSchema = null;
		}

		#region Cassandra Keyspace Server Operations

		public CassandraKeyspaceSchema GetSchema()
		{
			if (_cachedSchema == null)
				_cachedSchema = new CassandraKeyspaceSchema(_context.ExecuteOperation(new SimpleOperation<Apache.Cassandra.KsDef>(ctx => {
					return ctx.Session.GetClient().describe_keyspace(KeyspaceName);
				})));

			return _cachedSchema;
		}

		public IEnumerable<CassandraTokenRange> DescribeRing()
		{
			return _context.ExecuteOperation(new SimpleOperation<IEnumerable<CassandraTokenRange>>(ctx => {
				var tokenRanges = ctx.Session.GetClient(setKeyspace: false).describe_ring(KeyspaceName);
				return tokenRanges.Select(tr => new CassandraTokenRange(tr.Start_token, tr.End_token, tr.Endpoints));
			}));
		}

		#endregion
	}
}
