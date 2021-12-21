using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace RickApps.UploadFilesMVC.Controllers
{
    public class PhotoController : BaseController
    {
        private IWebHostEnvironment hostEnv;
        private string ServerDestinationPath { get; set; }

        public PhotoController(IUnitOfWork uow, IWebHostEnvironment env) : base(uow) 
        {
            hostEnv = env;
        }

        // We use the item number to determine the folder for uploaded files. Rather than put all pics
        // into a single folder, we will create a new folder for every 100 items. The intent is to keep
        // from having a very large number of files in a single folder which could slow retrieval.
        private void InitProperties(int Number)
        {
            // Remove the rightmost two digits from Number to determine our destination folder
            int leftmost = Number / 100;
            // Make the folder name be at least five characters long
            string folderName = string.Format("{0:00000}", leftmost);
            ServerDestinationPath = Path.Combine(hostEnv.WebRootPath, "\\pics\\", folderName);
            return;
        }

        public RedirectToActionResult DeleteImage(int itemID, int imageID)
        {
            Photo image = ((PhotoRepository)_repository.ItemPhotos).GetItemImages(itemID).SingleOrDefault(p => p.ID == imageID);
            ((PhotoRepository)_repository.ItemPhotos).Remove(image);
            _repository.Complete();

            return RedirectToAction("Edit", "Admin", new { ItemID = itemID, isPhoto = true });
        }

        [HttpPost]
        public ActionResult UploadFiles(int itemID, IFormFile[] files)
        {
            try
            {
                string inputFileName;
                string serverSavePath;
                List<string> newPics = new List<string>();
                Item item = ((ItemRepository)_repository.Items).GetItem(itemID);
                InitProperties(item.Number);
                // Get the next sequence number. Cannot do a count. Need to retrieve max number
                int start = item.Photos.Count > 0 ? (item.Photos.Max(p=>p.Sequence)) + 1 : 1;
                int seq = start;
                if (ModelState.IsValid)
                {
                    foreach (IFormFile file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Make sure the file is an image file
                            // Generate new name
                            inputFileName = String.Format("{0}-{1:D2}.jpg", item.Number, seq);
                            serverSavePath = Path.Combine(hostEnv.ContentRootPath, "Uploads",  inputFileName);
                            //Save file to server folder
                            using (FileStream stream = new FileStream(serverSavePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                newPics.Add(serverSavePath);
                            }
                            // Add the item to the database
                            Photo newImage = new Photo();
                            newImage.ItemID = item.ID;
                            newImage.Sequence = seq;
                            newImage.LinkToLargeImage = Path.Combine(ServerDestinationPath, inputFileName);
                            newImage.LinkToMediumImage = Path.Combine(ServerDestinationPath, "medium", inputFileName);
                            newImage.LinkToSmallImage = Path.Combine(ServerDestinationPath, "thumb", inputFileName);
                            item.Photos.Add(newImage);
                            seq++;
                        }
                        _repository.Complete();
                    }
                    // Resize the images. We generate a batch number to deal with legacy code from Access system
                     int numPhotos = ResizePhotos(newPics);
                    // Delete the original uploaded files
                    foreach (string s in newPics)
                        System.IO.File.Delete(s);
                }
            }
            catch (FormatException ex)
            {
                throw ex;
                //return Content(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Edit", "Admin", new { ItemID = itemID, isPhoto = true });
        }

        private int ResizePhotos(ICollection<string> newPics)
        {
            //Determine where everything is
            string sourcePath = Path.Combine(hostEnv.ContentRootPath, "\\newpics\\");
            string destPath = ServerDestinationPath;
            string procFile = "unknown";
            int count = 0;

            try
            {
                //Do we need to create our destination folder?
                if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);

                //Create sub folders to hold the three picture sizes
               if (!Directory.Exists(destPath + "medium")) Directory.CreateDirectory(destPath + "medium");
                if (!Directory.Exists(destPath + "thumb")) System.IO.Directory.CreateDirectory(destPath + "thumb");

                // Loop through the list of files to be resized
                TempData["message"] = "Images resized.";  // Gets overwritten if we have an exception
                foreach (var s in newPics)
                {
                    //Create a new name for our file. The name is based on the item number and sequence
                    string baseName = Path.GetFileNameWithoutExtension(s);
                    // Copy and rename the file to our destination folder
                    System.IO.File.Copy(s, destPath + baseName + ".jpg");
                    using (FileStream fullSizeImg = new FileStream(s, FileMode.Open, FileAccess.Read))
                    {
                        //Create three new image sizes
                        string saveName;
                        saveName = Path.Combine("thumb", baseName + ".jpg");
                        ResizeImage(fullSizeImg, destPath + saveName, 128);
                        saveName = Path.Combine("medium", baseName + ".jpg");
                        ResizeImage(fullSizeImg, destPath + saveName, 377);
                        count++;
                        fullSizeImg.Dispose();
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                TempData["message"] = String.Format("The file is not a valid image file: {0}", procFile);
            }
            catch (Exception ex)
            {
                TempData["message"] = "Something bad happened.";
            }
            return count;
        }

#pragma warning disable CA1416 // Validate platform compatibility
        /// <summary>
        /// Resize an image to a specified width. Aspect ratio is preserved.
        /// The source image is not altered.
        /// This will work if hosted on Windows. Not tried on other platforms.
        /// </summary>
        /// <param name="pngStream"></param>
        /// <param name="saveName"></param>
        /// <param name="imgWidth"></param>
        private void ResizeImage(FileStream pngStream, string saveName, int imgWidth)
        {
            using (var image = new Bitmap(pngStream))
            {
                // Calculate the new height of the image given its desired width
                int height = (int)Math.Round(image.Height * (imgWidth / (float)image.Width));
                var resized = new Bitmap(imgWidth, height);
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, imgWidth, height);
                    resized.Save(saveName, ImageFormat.Jpeg);
                }
            }
        }
#pragma warning restore CA1416 // Validate platform compatibility
    }
}