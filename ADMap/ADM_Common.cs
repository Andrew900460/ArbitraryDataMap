using System;
using System.IO;
using System.Collections.Generic;
//using UnityEngine;

namespace SerialMap {
	using static ADM_Util;

	public delegate void WriterFunction(BinaryWriter stream, object data);
	public delegate object ReaderFunction(BinaryReader stream);

	public class ADM_Common {

		private readonly static List<ADMType> MainDataTypeList = new List<ADMType>() {
			new ADMType(1, typeof(byte),        WriteByte, ReadByte),
			new ADMType(2, typeof(short),       WriteShort, ReadShort),
			new ADMType(3, typeof(int),         WriteInt, ReadInt),
			new ADMType(4, typeof(long),        WriteLong, ReadLong),
			new ADMType(5, typeof(float),       WriteFloat, ReadFloat),
			new ADMType(6, typeof(double),      null, null),
			new ADMType(7, typeof(bool),        null, null),
			new ADMType(8, typeof(string),      WriteString, ReadString),
			new ADMType(9, typeof(ADMList),     WriteList, ReadList),
			new ADMType(10,typeof(ADMap),       WriteCompound, ReadCompound),
			//new ADMType(32, typeof(float3),  WriteCompound, ReadCompound),
		};

		private readonly static SortedList<Type, ADMType> typeSortedList = 
			new SortedList<Type, ADMType>(new TypeComparer());

		private static SortedList<int, ADMType> typeIdSortedList = 
			new SortedList<int, ADMType>(100);

		public static SortedList<Type, ADMType> ADMTypesList => typeSortedList;
		public static SortedList<int, ADMType> ADMTypesByTypeID => typeIdSortedList;

		class TypeComparer : IComparer<Type> {
			public int Compare(Type x, Type y) {
				return x.GUID.CompareTo(y.GUID);
			}
		}

		static ADM_Common() {
			foreach(ADMType type in MainDataTypeList) {
				typeSortedList.Add(type.type, type);
				typeIdSortedList.Add(type.typeID, type);
			}
		}

		private static void WriteByte(BinaryWriter stream, object data) =>
			stream.Write((byte)data);

		private static object ReadByte(BinaryReader stream) => stream.ReadByte();

		private static void WriteShort(BinaryWriter stream, object data) =>
			stream.Write((short)data);

		private static object ReadShort(BinaryReader stream) => stream.ReadInt16();

		private static void WriteInt(BinaryWriter stream, object data) =>
			stream.Write((int)data);

		private static object ReadInt(BinaryReader stream) => stream.ReadInt32();

		private static void WriteLong(BinaryWriter stream, object data) =>
			stream.Write((long)data);

		private static object ReadLong(BinaryReader stream) => stream.ReadInt64();


		private static void WriteFloat(BinaryWriter stream, object data) =>
			stream.Write((float)data);

		private static object ReadFloat(BinaryReader stream) => stream.ReadSingle();

		//private static void WriteFloat3(BinaryWriter stream, object data) {
		//	Vector3 v = (Vector3)data;
		//	stream.Write(v.x);
		//	stream.Write(v.y);
		//	stream.Write(v.z);
		//}
		//private static object ReadFloat3(BinaryReader stream) =>
		//	new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());

		private static void WriteString(BinaryWriter stream, object data) =>
			stream.Write((string)data);

		private static object ReadString(BinaryReader stream) => stream.ReadString();

		private static void WriteCompound(BinaryWriter stream, object data) =>
			((ADMap)data).WriteToStream(stream);

		private static object ReadCompound(BinaryReader stream) {
			ADMap map = new ADMap();
			map.ReadFromStream(stream);
			return map;
		}

		private static void WriteList(BinaryWriter stream, object data) =>
			((ADMList)data).WriteToStream(stream);

		private static object ReadList(BinaryReader stream) {
			int ListTypeID = stream.ReadInt32();

			ADMList list = new ADMList(GetADMType(ListTypeID));
			list.ReadFromStream(stream);
			return list;
		}
	}
}
