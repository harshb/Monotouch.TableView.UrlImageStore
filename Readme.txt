
#Lazy loading of images in a Monotouch TableView

Xamarin has provided a sample of Lazy loading TableView images at https://github.com/xamarin/monotouch-samples/tree/master/LazyTableImages
However, I feel it is inefficiant because it does a full TableView.ReloadData () on its callback (HandleAppsCollectionChanged)

I found  Pavel Sich's library  https://github.com/sichy/UrlImageStore that used  Task Parallels from .NET 4 did a good job at downloading images on a background thread.
However, in the context of a UITableView, it was unaware of the rows to refresh after finising downloading a image in a cell.

I Modified  Sich's code  so that it could be used with a TableView UITableViewSource.

Basically you pass on the row number and section number of the cell holding the image to the ImageManager  and 
when the image has finished downloading, the callback returns you the row and the section number, which you then use to update the corrosponding row.



Usage
-----
<code>

public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
{
    string myUrl = "http://some image url ";
    UIImage image = ImageManager.Instance.GetImage(myUrl, indexPath.Row, indexPath.Section);

    if (image == null) // it is not available cached, so we will wait for it
    {
	this.InvokeOnMainThread(delegate
	{

	    NSIndexPath path = NSIndexPath.FromRowSection(theRow, thesection);
	    Controller.TableView.ReloadRows(new[] { path }, UITableViewRowAnimation.None);

	});
    }
    else
    {
	// it exists so show it here
	if (photoView != null)
	    photoView.Image = image;


    }
    return cell;

}

</code>