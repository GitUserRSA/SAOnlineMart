﻿namespace SAOnlineMart.Services.Interface
{
    public interface IFileService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);

        public bool DeleteImage(string imageFileName);

        public string GetRootPath(string ImagePath);
    }
}
