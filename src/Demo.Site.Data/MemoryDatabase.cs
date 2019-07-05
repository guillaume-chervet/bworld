using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Demo.Data.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Data.Mock
{
    public static class MemoryDatabase
    {
        private static readonly IList<Item> database = new List<Item>();

        static MemoryDatabase()
        {
        }

        public static IList<Item> Database
        {
            get
            {
                if (database.Count <= 0)
                {
                    Load();
                }
                return database;
            }
        }

        public static void SaveChange(MemorySession<ItemDataModelBase> memorySession)
        {
            lock (database)
            {
                foreach (ItemDataModel o in memorySession.DatabaseDelete)
                {
                    var item = Database.FirstOrDefault(d => d.Id == o.Id);
                    Database.Remove(item);
                }

                foreach (ItemDataModel itemDataModel in memorySession.DatabaseAdd)
                {
                    var item = new Item();
                    item.Index = itemDataModel.Index;
                    item.IsTemporary = itemDataModel.IsTemporary;
                    if (itemDataModel.Data != null)
                    {
                        item.Json = JsonConvert.SerializeObject(itemDataModel.Data);
                        //item.Type = itemDataModel.Data.GetType().FullName;
                    }
                    item.Module = itemDataModel.Module;
                    item.ParentId = itemDataModel.ParentId;
                    item.PropertyName = itemDataModel.PropertyName;
                    item.SiteId = itemDataModel.SiteId;
                    item.CreateDate = DateTime.Now;

                    if (string.IsNullOrEmpty(itemDataModel.Id))
                    {
                        item.Id = (Guid.NewGuid()).ToString();
                    }
                    else
                    {
                        item.Id = itemDataModel.Id;
                    }
                    itemDataModel.Id = item.Id;

                    Database.Add(item);
                    itemDataModel.Id = item.Id;
                }

                foreach (var itemDataModelBase in memorySession.DatabaseAdd)
                {
                    var itemDataModel = (ItemDataModel) itemDataModelBase;
                    if (itemDataModel.Childs != null)
                    {
                        foreach (var dataModel in itemDataModel.Childs)
                        {
                            if (dataModel.Parent == null)
                            {
                                dataModel.ParentId = itemDataModel.Id;
                            }
                            // TODO Code rapide moche + risque bug
                            var item = Database.FirstOrDefault(d => d.Id == dataModel.Id);
                            item.ParentId = itemDataModel.Id;
                        }
                    }
                }

                memorySession.DatabaseDelete.Clear();
                memorySession.DatabaseAdd.Clear();

                foreach (var itemDataModelBase in memorySession.DatabaseLoaded)
                {
                    var itemDataModel = (ItemDataModel) itemDataModelBase;
                    var item = Database.FirstOrDefault(d => d.Id == itemDataModel.Id);

                    if (item == null)
                    {
                        continue;
                    }

                    item.Index = itemDataModel.Index;
                    item.IsTemporary = itemDataModel.IsTemporary;
                    if (itemDataModel.Data != null)
                    {
                        item.Json = JsonConvert.SerializeObject(itemDataModel.Data);
                        //item.Type = itemDataModel.Data.GetType().FullName;
                    }
                    item.Module = itemDataModel.Module;
                    item.ParentId = itemDataModel.ParentId;
                    item.PropertyName = itemDataModel.PropertyName;
                    item.SiteId = itemDataModel.SiteId;
                    item.UpdateDate = DateTime.Now;
                    item.Id = itemDataModel.Id;
                }

                Save();
            }
        }

        public static string GetPath(string file)
        {
            if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
            {
                var temp = AppDomain.CurrentDomain.RelativeSearchPath.Replace(@"\bin", "");
                return Path.Combine(temp, @"App_Data", file);
            }
            return Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), file);
        }

        public static void Load()
        {
            var path = GetPath("bdd.json");

            var contentInit = string.Empty;

            if (File.Exists(path))
            {
                contentInit = File.ReadAllText(path);
            }

            var pathInit = GetPath(@"bdd-init.json");
            if (File.Exists(pathInit))
            {
                contentInit = File.ReadAllText(pathInit);
            }

            if (!string.IsNullOrEmpty(contentInit))
            {
                var list = JsonConvert.DeserializeObject<List<Item>>(contentInit);
                foreach (var elem in list)
                {
                    database.Add(elem);
                }
            } /* else
                {
                    // Site 
                    var site = new ItemDataModel
                    {
                        Id = "1",
                        Json = @"{CultureId : '11',Name : 'myworld',Title :'MyWorld'}",
                        Module = "Site"
                    };
                    databaseAdd.Add(site);

                    // Item
                    var list = new List<ItemDataModel>();

                    const string json1 = @"{ elements: [            { 'type': 'h1', 'data': 'Accueil' },
            { 'type': 'p',  'data': 'Myworld vous permet de créer votre site internet pour votre activité simplement. Aucune connaissance particulière en informatique n\'est requise! Nous partons du principe que tout le monde ne maitrise pas l\'informatique et n\'a pas assez de temps libre pour représenter son activité sur le web.' }]}";

                    const string json2 = @" {elements:[            { 'type': 'h1', 'data': 'Contacts' },
            { 'type': 'p',  'data': 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.' },
{ 'type': 'file',  'data': '[]' }]}";

                    const string json3 = @" {elements:[            { 'type': 'h1', 'data': 'A propos' },
            { 'type': 'p',  'data': 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.' },
{ 'type': 'file',  'data': '[]' }]}";

                    var itemDataModel1 = new ItemDataModel
                    {
                        PropertyName = "MenuItems",
                        Module = "Free",
                        SiteId = "1",
                        Index = 0,
                        Id = "2",
                        ParentId = "1",
                        Json = json1
                    };
                    list.Add(itemDataModel1);
                    databaseAdd.Add(itemDataModel1);

                    var itemDataModel2 = new ItemDataModel
                    {
                        PropertyName = "MenuItems",
                        Module = "Free",
                        SiteId = "1",
                        Index = 1,
                        Id = "3",
                        ParentId = "1",
                        Json = json2
                    };
                    list.Add(itemDataModel2);
                    databaseAdd.Add(itemDataModel2);

                    var itemDataModel3 = new ItemDataModel
                    {
                        PropertyName = "MenuItems",
                        Module = "Free",
                        SiteId = "1",
                        Index = 2,
                        Id = "4",
                        ParentId = "1",
                        Json = json3
                    };
                    list.Add(itemDataModel3);
                    databaseAdd.Add(itemDataModel3);

                    SaveChange();
                }*/
        }

        public static object GetItemData(Item item)
        {
            if (item == null || string.IsNullOrEmpty(item.Json))
            {
                return null;
            }

            var assembly =
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.ManifestModule.Name == "Demo.Business.dll")
                    .FirstOrDefault();

            if (assembly != null)
            {
                if (!string.IsNullOrEmpty(item.Type))
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.FullName == item.Type)
                        {
                            return JsonConvert.DeserializeObject(item.Json, type);
                        }
                    }
                }
                else
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.Name == item.Module + "BusinessModel")
                        {
                            return JsonConvert.DeserializeObject(item.Json, type);
                        }
                    }
                }
            }

            return null;
        }

        public static void Save()
        {
            var path = GetPath(@"bdd.json");

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var dataJson = JsonConvert.SerializeObject(database, Formatting.Indented, jsonSerializerSettings);

            File.WriteAllText(path, dataJson);
        }
    }
}