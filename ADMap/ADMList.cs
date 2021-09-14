using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SerialMap {
	using static ADM_Util;
	public class ADMList {
		private List<ADMapElement> dataList = new List<ADMapElement>();

		private ADMType listType;

		public int Length => dataList.Count;
		public ADMType ListType => listType;

		public List<ADMapElement> DataList => dataList;

		public ADMList(ADMType listType) {
			this.listType = listType;
		}

		public ADMList(IList initList) {
			if(initList.Count > 0) {
				listType = GetADMTypeFromObject(initList[0]);

				for(int i = 0; i < initList.Count; i++) {
					var element = initList[i];
					object data;

					if(element is ADMConvertible)
						data = ((ADMConvertible)element).GenerateMap();
					else
						data = element;

					dataList.Add(new ADMapElement(listType, data));
				}
			}
		}

		public void Add(object data) {
			dataList.Add(new ADMapElement(listType, data));
		}

		public ADMapElement Get(int index) {
			return dataList[index];
		}

		public static void PopulateList<T>(ADMList sourceList, IList<T> destinationList) where T : ADMConvertible, new() {
			destinationList.Clear();
			for(int i = 0; i < sourceList.DataList.Count; i++) {
				var mapData = sourceList.DataList[i].data;
				var trueData = ((ADMap)mapData).GenerateFromMap<T>();
				destinationList.Add(trueData);
			}
		}

		public static void PopulateList(ADMList sourceList, IList destinationList) {
			destinationList.Clear();
			for(int i = 0; i < sourceList.dataList.Count; i++) {
				var data = sourceList.dataList[i].data;

				destinationList.Add(data);
			}
		}

		public void ReadFromStream(BinaryReader reader) {
			int ListLength = reader.ReadInt32();
			for(int i = 0; i < ListLength; i++) {
				object data = listType.ReaderFunction(reader);
				Add(data);
			}
		}

		public void WriteToStream(BinaryWriter writer) {
			writer.Write(listType.typeID); // writing the type of the list itself
			writer.Write(dataList.Count); // length of list
			for(int i = 0; i < dataList.Count; i++)
				dataList[i].dataType.WriterFunction(writer, dataList[i].data);
		}

		public static ADMList ToADMList(IList list) {
			return new ADMList(list);
		}

	}


}
