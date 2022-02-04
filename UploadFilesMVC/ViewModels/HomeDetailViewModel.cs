namespace RickApps.UploadFilesMVC.ViewModels
{
    using RickApps.UploadFilesMVC.Models;
    using System;
    using System.Collections.Generic;

    public class HomeDetailViewModel
    {
        private Item _item;
        public HomeDetailViewModel(Item item)
        {
            _item = item;
            CreateImageList();
        }

        public string PageTitle { get; set; }

        public int ItemID
        {
            get { return _item.ID; }
        }

        public string Name
        {
            get { return _item.Name; }
        }

        public string FormattedPrice
        {
            get { return _item.FormattedPrice; }
        }

        public string Description
        {
            get { return _item.Description; }
        }

        public int Number
        {
            get { return _item.Number; }
        }

        public IEnumerable<string> ItemImages
        {
            get;

            private set;

        }

        public IEnumerable<string> Thumbnails
        {
            get;

            private set;
        }

        private void CreateImageList()
        {
            List<string> images = new List<string>();
            List<string> thumbs = new List<string>();

            foreach (var image in _item.Photos)
            {
                images.Add(String.Format("{0}", image.LinkToLargeImage));
                thumbs.Add(String.Format("{0}", image.LinkToMediumImage));
            }

            if (images.Count == 0)
            {
                // Put a stub image in
                images.Add("~/images/MockUp/272/ring1.jpg");
                thumbs.Add("~/images/MockUp/272/ring1.jpg");
            }

            ItemImages = images;
            Thumbnails = thumbs;
        }


    }
}
