﻿using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Stub
{
    public class StubbedLaboratory : ICRUDService<Laboratory>
    {
        public static CollectionManager<Laboratory> LaboratoryCollection { get; set; }

        readonly static string laboratoryPath;

        static StubbedLaboratory()
        {
            string directoryPath;
            string filePathUsers;

#if WINDOWS
                directoryPath = Path.Combine("C:", "Soft_Etalonnage", "Configuration");
                filePathUsers = Path.Combine(directoryPath, "laboratories.xml");
#else
            directoryPath = FileSystem.AppDataDirectory;
            filePathUsers = Path.Combine(directoryPath, "laboratories.xml");
#endif

            LaboratoryCollection = new CollectionManager<Laboratory>();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                Directory.CreateDirectory(directoryPath);
                LaboratoryCollection.SaveToFile(filePathUsers);
            }
            else
            {
                Console.WriteLine($"Directory exists: {directoryPath}");
                if (!File.Exists(filePathUsers))
                {
                    Console.WriteLine($"File not found: {filePathUsers}");
                    InitializeLaboratory();
                    LaboratoryCollection.SaveToFile(filePathUsers);
                }
                else
                {
                    LaboratoryCollection.LoadFromFile(filePathUsers);
                    if (LaboratoryCollection.Items.Count == 0)
                    {
                        Console.WriteLine("No user found, creating default users");
                        InitializeLaboratory();
                        LaboratoryCollection.SaveToFile(filePathUsers);
                    }
                }
            }
            // On sauvegarde le Path de manière globale à l'application ici, puisqu'on ne peut pas l'utiliser avant le build de l'application
            laboratoryPath = filePathUsers;
        }

        private static void InitializeLaboratory()
        {
            // Lister toutes les ressources disponibles
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DAL.Image.tampon_di.png";

            byte[] imageData;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Resource not found: " + resourceName);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    imageData = ms.ToArray();
                }
            }

            // Créer l'objet Laboratory avec le cachet d'entreprise
            Laboratory newLaboratory = new Laboratory(
                name: "Dufournier Industries",
                adress: "1 Avenue des Mineurs 63570 AUZAT LA COMBELLE",
                laboritoryLocation: "Local étalonnage à l'adresse du laboratoire",
                cachetEntreprise: new Picture(imageData)
            );

            LaboratoryCollection.Items.Add(newLaboratory);
        }

        // Méthodes sans paramètre customerName
        public async Task<Pagination<Laboratory>> GetAsyncAllObject(int index, int count)
        {
            return await GetAsyncAllObject(index, count, null);
        }

        public async Task<Laboratory> GetObjectAsyncById(string id)
        {
            return await GetObjectAsyncById(id, null);
        }

        public async Task<bool> CreateObject(Laboratory genericObject)
        {
            return await CreateObject(genericObject, null);
        }

        public async Task<Laboratory> UpdateObject(Laboratory genericObject)
        {
            return await UpdateObject(genericObject, null);
        }

        public async Task<bool> DeleteObject(string id)
        {
            return await DeleteObject(id, null);
        }

        // Méthodes avec paramètre customerName
        public async Task<Pagination<Laboratory>> GetAsyncAllObject(int index, int count, string? customerName)
        {
            var laboratories = LaboratoryCollection.Items.Skip(index * count).Take(count).ToList();
            var pagination = new Pagination<Laboratory>
            {
                TotalCount = LaboratoryCollection.Items.Count,
                Index = index,
                Cout = count,
                Items = laboratories
            };
            return await Task.FromResult(pagination);
        }

        public async Task<Laboratory> GetObjectAsyncById(string id, string? customerName)
        {
            var laboratory = LaboratoryCollection.Items.FirstOrDefault(u => u.Name == id);
            return await Task.FromResult(laboratory ?? null!);
        }

        public async Task<bool> CreateObject(Laboratory genericObject, string? customerName)
        {
            if (LaboratoryCollection.Items.Any(u => u.Name == genericObject.Name))
            {
                return await Task.FromResult(false);
            }

            LaboratoryCollection.Items.Add(genericObject);
            LaboratoryCollection.SaveToFile(laboratoryPath);
            return await Task.FromResult(true);
        }

        public async Task<Laboratory> UpdateObject(Laboratory genericObject, string? customerName)
        {
            var existingLaboratory = LaboratoryCollection.Items.FirstOrDefault(u => u.Name == genericObject.Name);
            if (existingLaboratory != null)
            {
                existingLaboratory.Name = genericObject.Name;
                existingLaboratory.Address = genericObject.Address;
                existingLaboratory.LaboritoryLocation = genericObject.LaboritoryLocation;
                existingLaboratory.CachetEntreprise = genericObject.CachetEntreprise;

                LaboratoryCollection.SaveToFile(laboratoryPath);
                return await Task.FromResult(existingLaboratory);
            }
            return await Task.FromResult<Laboratory>(null!);
        }

        public async Task<bool> DeleteObject(string id, string? customerName)
        {
            var existingLaboratory = LaboratoryCollection.Items.FirstOrDefault(u => u.Name == id);
            if (existingLaboratory != null)
            {
                LaboratoryCollection.Items.Remove(existingLaboratory);
                LaboratoryCollection.SaveToFile(laboratoryPath);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}