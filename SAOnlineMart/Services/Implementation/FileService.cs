namespace SAOnlineMart.Services.Implementation
{
    public class FileService
    {
        private IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool DeleteImage(string imageFileName) //Function to handle product image deletion
        {
            try
            {
                var wwwPath = this._webHostEnvironment.WebRootPath; //Get the root path to the folder of the project
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName); //Append the root path with the uploads folder and image name to get the full path
                if (System.IO.File.Exists(path)) //delete the file if it exists
                {
                    System.IO.File.Delete(path); //delete file
                    return true;
                }
                return false;
            }
            catch
            (Exception ex)
            {
                return false;
            }

        }

        public Tuple<int, string> SaveImage(IFormFile imageFile) //Save the product image
        {
            try
            {

                var contentPath = this._webHostEnvironment.WebRootPath;
                //path
                var path = Path.Combine(contentPath, "Uploads");
                Console.WriteLine(path);
                if (!Directory.Exists(path)) //If the Uploads directory does not exist create it
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    Console.WriteLine("path was not created!"); //In the event it does not work
                }

                //Check allowed extensions
                var extensions = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".PNG" };

                if (!allowedExtensions.Contains(extensions))
                {
                    string msg = string.Format("Only {0} extensions allowed", string.Join(" ", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString(); //Generate a new name for the image
                var newFileName = uniqueString + extensions; //Append the file path to the new name
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create); //Save the image to the folder
                imageFile.CopyTo(stream);
                stream.Close();

                return new Tuple<int, string>(1, newFileName);


            }
            catch
            (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }
        }
    }
}
