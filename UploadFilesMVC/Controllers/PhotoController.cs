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
            ServerDestinationPath = Path.Join("\\pics\\", folderName);
            return;
        }

        public RedirectToActionResult DeleteImage(int ID, int imageID)
        {
            Photo image = ((PhotoRepository)_repository.ItemPhotos).GetItemImages(ID).SingleOrDefault(p => p.ID == imageID);
            ((PhotoRepository)_repository.ItemPhotos).Remove(image);
            _repository.Complete();

            return RedirectToAction("Edit", "Admin", new { ID = ID, isPhoto = true });
        }

        [HttpPost]
        public ActionResult UploadFiles(int itemID, IFormFile[] files)
        {
            try
            {
                string inputFileName;
                string serverSavePath;
                List<string> uploadedFiles = new List<string>(); 
                Item item = ((ItemRepository)_repository.Items).GetItem(itemID);
                InitProperties(item.Number);
                // Get the next sequence number. Cannot do a count. Need to retrieve max number
                int start = item.Photos.Count > 0 ? (item.Photos.Max(p=>p.Sequence)) + 1 : 1;
                int seq = start;
                if (ModelState.IsValid)
                {
                    foreach (IFormFile file in files)
                    {
                        // Make sure file is within valid size
                        if (file.Length > 0 && file.Length < 4000000)  // About 4MB
                        {
                            // Check file extension to make sure it is allowed.
                            // Generate a name for the uploaded file. Don't keep original name
                            inputFileName = String.Format("{0}-{1:D2}.jpg", item.Number, seq);
                            // We don't know much about the uploaded file. Store it outside of our website
                            serverSavePath = Path.Combine(Path.GetTempPath(),  inputFileName);
                            // Write the uploaded file to serverSavePath. We know it is within our
                            // valid size range and has acceptable extension.
                            using (FileStream stream = new FileStream(serverSavePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                // Make a list of all files that have been uploaded. The user can select more
                                // than one file at a time.
                                uploadedFiles.Add(serverSavePath);
                            }
                            // We have not made copies of the uploaded files yet, but we are going to update our model.
                            // We can always not commit changes to db if the files don't copy.
                            Photo newImage = new Photo();
                            newImage.ItemID = item.ID;
                            newImage.Sequence = seq;
                            newImage.LinkToLargeImage = Path.Combine(ServerDestinationPath, inputFileName);
                            newImage.LinkToMediumImage = Path.Combine(ServerDestinationPath, "medium", inputFileName);
                            newImage.LinkToSmallImage = Path.Combine(ServerDestinationPath, "thumb", inputFileName);
                            item.Photos.Add(newImage);
                            seq++;
                        }
                        else
                        {
                            // ignore the file. If you were a good programmer, you would tell the user why.
                            continue;
                        }
                    }
                    // Resize the images. 
                    int numPhotos = ResizePhotos(uploadedFiles, item);
                    // All went well, save to the database
                    _repository.Complete();
                    // Delete the original uploaded files
                    foreach (string s in uploadedFiles)
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
            return RedirectToAction("Edit", "Admin", new { ID = itemID, isPhoto = true });
        }

        /// <summary>
        /// Make copies of the uploaded photos, resize them, and store
        /// to a location accessible to the website.
        /// </summary>
        /// <param name="newPics"></param>
        /// <returns></returns>
        private int ResizePhotos(ICollection<string> newPics, Item item)
        {
            int count = 0;
            if (newPics.Count() == 0) return count;

            //Determine where we should put stuff
            string largePath = Path.Join(hostEnv.WebRootPath, Path.GetDirectoryName(item.Photos.First<Photo>().LinkToLargeImage));
            string mediumPath = Path.Join(hostEnv.WebRootPath, Path.GetDirectoryName(item.Photos.First<Photo>().LinkToMediumImage));
            string thumbPath = Path.Join(hostEnv.WebRootPath, Path.GetDirectoryName(item.Photos.First<Photo>().LinkToSmallImage));
            string procFile = "unknown";

            try
            {
                //Do we need to create our destination folder?
                if (!Directory.Exists(largePath)) Directory.CreateDirectory(largePath);
                //Create sub folders to hold the three picture sizes
                if (!Directory.Exists(mediumPath)) Directory.CreateDirectory(mediumPath);
                if (!Directory.Exists(thumbPath)) Directory.CreateDirectory(thumbPath);

                // Loop through the list of files to be resized
                TempData["message"] = "Images resized.";  // Gets overwritten if we have an exception
                foreach (var s in newPics)
                {
                    //Create a new name for our file. The name is based on the item number and sequence
                    string baseName = Path.GetFileName(s);
                    // Copy and rename the file to our destination folder
                    System.IO.File.Copy(s, Path.Join(largePath, baseName));
                    using (FileStream fullSizeImg = new FileStream(s, FileMode.Open, FileAccess.Read))
                    {
                        //Create two new image sizes
                        string saveName;
                        saveName = Path.Combine(thumbPath, baseName);
                        ResizeImage(fullSizeImg, saveName, 128);
                        saveName = Path.Combine(mediumPath, baseName);
                        ResizeImage(fullSizeImg, saveName, 377);
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