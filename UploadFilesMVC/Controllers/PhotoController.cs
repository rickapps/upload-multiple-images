using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using System;
using System.Collections.Generic;
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
        private void InitProperties(int batchNo)
        {
            ServerDestinationPath = String.Format("\\pics\\{0}\\{1}\\", folderYear, BatchNumber);
            return;
        }

        public RedirectToActionResult DeleteImage(int itemID, int imageID)
        {
            Photo image = ((PhotoRepository)_repository.ItemPhotos).GetItemImages(itemID).SingleOrDefault(p => p.PhotoID == imageID);
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
                Item item = ((ItemRepository)_repository.Items).Get(itemID);
                InitProperties(item.ItemBatchNo ?? 10101);
                // Get the next sequence number. Cannot do a count. Need to retrieve max number
                int start = (item.Photos.Max(p => p.Sequence)) + 1;
                int seq = start;
                if (ModelState.IsValid)
                {
                    IWebHostEnvironment host;
                    foreach (IFormFile file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Make sure the file is an image file
                            // Generate new name
                            inputFileName = String.Format("{0}-{1:D2}.jpg", item.Number, seq);
                            serverSavePath = Path.Combine(hostEnv.WebRootPath, "NewPics" + inputFileName);
                            //Save file to server folder
                            using (FileStream stream = new FileStream(serverSavePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                newPics.Add(serverSavePath);
                            }
                            // Add the item to the database
                            Photo newImage = new Photo();
                            newImage.ItemID = item.ItemID;
                            newImage.Sequence = seq;
                            newImage.LinkToLargeImage = Path.Combine(ServerDestinationPath, inputFileName);
                            newImage.LinkToMediumImage = Path.Combine(ServerDestinationPath, String.Format("272\\{0}-{1:D2}w272.jpg", item.Number, seq));
                            newImage.LinkToSmallImage = Path.Combine(ServerDestinationPath, String.Format("128\\{0}-{1:D2}w128.jpg", item.Number, seq));
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
            string sourcePath = Server.MapPath("\\newpics\\");
            string destPath = Server.MapPath(ServerDestinationPath);
            string procFile = "unknown";
            int count = 0;

            try
            {
                //Do we need to create our destination folder?
                if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);

                //Create sub folders to hold the three picture sizes
                if (!Directory.Exists(destPath + "048")) Directory.CreateDirectory(destPath + "048");
                if (!Directory.Exists(destPath + "128")) Directory.CreateDirectory(destPath + "128");
                if (!Directory.Exists(destPath + "272")) System.IO.Directory.CreateDirectory(destPath + "272");

                // Loop through the list of files to be resized
                System.Drawing.Image fullSizeImg;
                TempData["message"] = "Images resized.";  // Gets overwritten if we have an exception
                foreach (var s in newPics)
                {
                    //Create a new name for our file. The name is based on the item number and sequence
                    string baseName = Path.GetFileNameWithoutExtension(s);
                    fullSizeImg = System.Drawing.Image.FromFile(s);

                    //Rename and copy the file to our destination path
                    fullSizeImg.Save(destPath + baseName + ".jpg");
                    //Create three new image sizes
                    string saveName;
                    saveName = Path.Combine("048", baseName + "w048.jpg");
                    ResizeImage(fullSizeImg, destPath + saveName, 40);
                    saveName = Path.Combine("128", baseName + "w128.jpg");
                    ResizeImage(fullSizeImg, destPath + saveName, 108);
                    saveName = Path.Combine("272", baseName + "w272.jpg");
                    ResizeImage(fullSizeImg, destPath + saveName, 377);
                    count++;
                    fullSizeImg.Dispose();
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

        /// <summary>
        /// Usage: photo/dothumbs/?ItemNo=12345&BatchNo=bhYMMDD
        /// This is here for legacy access system
        /// </summary>
        public ActionResult DoThumbs(string ItemNo, string BatchNo)
        {
            // Make sure we got two values
            if (String.IsNullOrWhiteSpace(ItemNo) || String.IsNullOrWhiteSpace(BatchNo))
            {
                TempData["message"] = "Incorrect call";
                return View();
            }
            if (BatchNo.Length != 7)
            {
                TempData["message"] = "Bad batch number";
                return View();
            }
            //ResizePhotos(ItemNo);

            return View();
        }

        private bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// Resize the image to one of three predetermined sizes
        /// </summary>
        /// <param name="img"></param>
        /// <param name="saveName"></param>
        /// <param name="imgSize"></param>
        private void ResizeImage(System.Drawing.Image img, string saveName, int imgSize)
        {
            Image sizedImage;
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            // Calculate the new height of the image given its desired width
            int height = img.Height * imgSize / img.Width;
            sizedImage = img.GetThumbnailImage(imgSize, height, myCallback, IntPtr.Zero);
            sizedImage.Save(saveName);
        }

        private void ResizeImage(string source, string saveName, int imgWidth)
        {
            WebImage myImage = new WebImage(source);
            // Calculate the new height of the image given its desired width
            int height = (int)Math.Round(myImage.Height * (imgWidth / (float)myImage.Width));
            myImage.Resize(imgWidth, height, true, true);
            myImage.Save(saveName);
        }
    }
}