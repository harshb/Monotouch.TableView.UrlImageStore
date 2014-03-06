using System;
using System.Runtime.InteropServices;

using MonoTouch.UIKit;


//Comments: Harsh Bhasin 3/6/2014
//I Modified  code from sichy / UrlImageStore https://github.com/sichy/UrlImageStore
//so that it could be used with a TableView UITableViewSource
//Basically you pass on the row number and section number of the cell holding the image to the ImageManager 
// when the image is finished downloading, the callback returns you the row and the section number
//then you call the NSIndexPath.FromRowSection to update just the one row that needs to be updated

namespace Monotouch.TableView.UrlImageStore
{
	public class ImageManager : IUrlImageUpdated
	{
		public delegate void ImageLoadedDelegate(string id, UIImage image, int row, int section);
		public event ImageLoadedDelegate ImageLoaded;

		UrlImageStore imageStore;

		private ImageManager()
		{
			imageStore = new UrlImageStore ("myImageStore", processImage);                                  
		}

		private static ImageManager instance;

		public static ImageManager Instance
		{
			get
			{
				if (instance == null)
					instance = new ImageManager ();

				return instance;
			}   
		}

		// this is the actual entrypoint you call
		public UIImage GetImage(string imageUrl, int row, int section)
		{
			return imageStore.RequestImage (imageUrl, imageUrl, this, row, section);
		}

		public void UrlImageUpdated (string id, UIImage image, int row, int section)
		{
			// just propagate to upper level
			if (this.ImageLoaded != null)
				this.ImageLoaded(id, image, row, section);
		}

		// This handles our ProcessImageDelegate
		// just a simple way for us to be able to do whatever we want to our image
		// before it gets cached, so here's where you want to resize, etc.
		UIImage processImage(string id, UIImage image)
		{
			return image;
		}  

		public void DeleteImages()
		{
			imageStore.DeleteCachedFiles ();
		}

	}
}

