using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace SerialMap {
	using static ADM_Util;
	public class ADMap : IEnumerable<KeyValuePair<string, ADMap.MapUnit>> {

		private SortedDictionary<string, MapUnit> rootMap =
			new SortedDictionary<string, MapUnit>();

		public class MapUnit {
			public ADMType dataType;
			public object data;
			public MapUnit(ADMType type, object data) =>
				(dataType, this.data) = (type, data);
		}

		public void SetData<T>(string name, T data) {
			ADMType type;
			//try { 
			type = GetADMType<T>();
			//}
			//catch(Exception) { throw InvalidDataType; }
			rootMap.Add(name, new MapUnit(type, data));
		}

		public T GetData<T>(string name) {
			return (T)rootMap[name].data;
		}

		public void WriteToStream(BinaryWriter writer) {
			foreach(var kv in rootMap) {
				MapUnit unit = kv.Value;
				ADMType type = unit.dataType;
				writer.Write(type.typeID);
				writer.Write(kv.Key);
				type.WriterFunction(writer, kv.Value.data);
			}
			writer.Write(0);
		}

		public void ReadFromStream(BinaryReader reader) {
			rootMap.Clear();
			int typeID;
			while((typeID = reader.ReadInt32()) != 0) {
				string key = reader.ReadString();
				var dataType = GetADMType(typeID);
				MapUnit unit = new MapUnit(dataType, dataType.ReaderFunction(reader));
				rootMap.Add(key, unit);
			}
		}

		public void PrintEntireMap(int level = 0) {
			string indentString = new string(' ', level * 4);
			foreach(var element in rootMap) {
				string keyAndTypeString = $"{indentString}\"{element.Key}\" - type:{element.Value.dataType.type.Name} = ";
				if(element.Value.data is ADMap) {
					Console.WriteLine($"{keyAndTypeString}{{");
					((ADMap)element.Value.data).PrintEntireMap(level + 1);
					Console.WriteLine($"{indentString}}}");
				}
				else if(element.Value.data is ADMList) {
					ADMList list = (ADMList)element.Value.data;
					Console.WriteLine($"{keyAndTypeString}[{list.Length} of {list.ListType.type}] {{");
					for(int i = 0; i < list.Length; i++) {
						var data = list.Get(i).data;
						if(data is ADMap) {
							Console.WriteLine($"{indentString}[{i}] = {{");
							((ADMap)data).PrintEntireMap(level + 1);
							Console.WriteLine($"{indentString}}}");
						}
						else
							Console.WriteLine($"{indentString} {list.Get(i).data},");
					}
					Console.WriteLine($"{indentString}}}");
				}
				else {
					string str = $"{keyAndTypeString}{element.Value.data}";
					Console.WriteLine(str);
				}

			}
		}

		public IEnumerator<KeyValuePair<string, MapUnit>> GetEnumerator() => rootMap.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => rootMap.GetEnumerator();

		private static ArgumentException InvalidDataType =
			new ArgumentException("Trying to add invalid data type to ADM");
	}
}
