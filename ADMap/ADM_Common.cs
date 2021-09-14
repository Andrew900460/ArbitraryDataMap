using System;
using System.IO;
using System.Collections.Generic;
//using UnityEngine;

namespace SerialMap {
	using static ADM_Util;

	public delegate void WriterFunction(BinaryWriter stream, object data);
	public delegate object ReaderFunction(BinaryReader stream);

	// Where the "base types" of ADMaps are defined.
	// Any data that could be composed of many of these "base types" would be held inside the "ADMap" type.

	// This is what allows you to save any information you want, in a way that is "order agnostic".
	// Meaning, the order at which data is saved to file doesn't matter, because the data is accessed by a string name.

	// You may add your own base types to this list if you want.
	// Like Vectors, Colors, Quaternions, Special ID values, etc.
	// Each type has an integer "type id" which is used to identify that particular data type in the file.
	// Each id HAS to be unique, so the systems knows which type is which in file.

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

		// Will automatically run before any of the above variables are accessed.
		// It's for initialization
		static ADM_Common() {
			foreach(ADMType type in MainDataTypeList) {
				typeSortedList.Add(type.type, type);
				typeIdSortedList.Add(type.typeID, type);
			}
		}

		// These static functions are what are used to govern how the base types are saved to file.
		// If you make your own base type, you will need to make your own "Write" and "Read" function as well.
		// And then include those function named in the base type definion above ^ "MainDataTypeList"

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
