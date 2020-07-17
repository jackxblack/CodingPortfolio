using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.ShellExtensions;
using System.IO;

//This is a very simple console app that edits metadata of all files in provided folder to provided string. I created it as audiobooks often didn't have metadata filled in a way that would be handled nicely by audiobook app I was using, so I've figured if I just change it all it would
//work. And it did.

namespace Mp3MetaReader
{
    class Program
    {    
        [STAThread]
        static void Main(string[] args)
        {
            //We start with gathering the folder where the files we want to change are stored. If the path provided by the user doesn't end with '\', we add one.
            Console.WriteLine("Enter folder path:");
            List<string> arrHeaders = new List<string>();
            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder objFolder;
            string folder = @Console.ReadLine();
            if (folder[folder.Length - 1] != '\\')
            {
                //Console.WriteLine("Provided path didn't end with \\, adding one.");
                folder += '\\';
            }

            //Then we collect the title from the file. It's a very important part as everything we change will be based on that.
            Console.WriteLine("Please provide book title. Files and their properties will be remade to Title - number");
            string title = Console.ReadLine();

            objFolder = shell.NameSpace(folder);            

            //We count the amount of files in this folder, as it affects amount of 0s that will appear in final file name and metadata.
            int itemsCount = 0;
            foreach (Shell32.FolderItem2 item in objFolder.Items())
            {
                itemsCount++;
            }

            //This just calculates how many 0s will need based on amount of files gathered above. The logic is that if there are less than 10 items, you don't need 0 at all, for 10 and more you will need one 0 for 1-9, for 100 and more two or one zeros etc. The 0's are required for
            //alphabetical sorting.
            int amountOfZeros = itemsCount.ToString().Length - 1;
            
            if (itemsCount > 0)
            {
                int loopIteration = 1;

                foreach (Shell32.FolderItem2 item in objFolder.Items())
                {
                    string filePath = folder + @objFolder.GetDetailsOf(item, 0);    //0 is the name property of the file (item). It doesn't return the extension though, thus +.mp3 in line below.
                    ShellFile file = ShellFile.FromFilePath(filePath + ".mp3");     //This builds actual reference to the file.
                    ChangeFile(file, amountOfZeros, loopIteration, folder, title);  //Method is below
                    loopIteration++; 
                }
            }
            else
            {
                Console.WriteLine("There are no items in provided folder.");
            }

        }

        static void ChangeFile(ShellFile file, int numberOfZeros, int loopIteration, string folder, string title)
        {
            //TODO: Handle read only property on files.
            //This is ugly and probably can be handled in a better way, but it works. Depending on how many 0s we need for this folder and how many files we've already affected, we rename the file to name given by the user plus required amount of 0s for the alphabetical sorting.
            switch (numberOfZeros)
            {
                case 0:
                    file.Properties.System.Music.Artist.Value = new string[] { loopIteration.ToString() + " - " + title };
                    file.Properties.System.Music.AlbumTitle.Value = loopIteration.ToString() + " - " + title;
                    file.Properties.System.Title.Value = loopIteration.ToString() + " - " + title;
                    File.Move(file.Path, folder + loopIteration.ToString() + " - " + title + ".mp3");                       //The Move() method is the easiest way to rename a file, as you declare the file's name in the new path, as well as its location.
                    break;
                case 1:
                    if (loopIteration < 10)
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { '0' + loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = '0' + loopIteration.ToString() + " - " +  title;
                        file.Properties.System.Title.Value = '0' + loopIteration.ToString() + " - " +  title;
                        File.Move(file.Path, folder + '0' + loopIteration.ToString() + " - " +  title + ".mp3");
                    }
                    else
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    break;
                case 2:
                    if (loopIteration < 10)
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { "00" + loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = "00" + loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = "00" + loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + "00" + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    else if (loopIteration < 100)
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { '0' + loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = '0' + loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = '0' + loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + '0' + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    else
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    break;
                case 3:
                    if (loopIteration < 10)
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { "000" + loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = "000" + loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = "000" + loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + "000" + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    else if (loopIteration < 100)
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { "00" + loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = "00" + loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = "00" + loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + "00" + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    else if (loopIteration < 1000)
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { '0' + loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = '0' + loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = '0' + loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + '0' + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    else
                    {
                        file.Properties.System.Music.Artist.Value = new string[] { loopIteration.ToString() + " - " + title };
                        file.Properties.System.Music.AlbumTitle.Value = loopIteration.ToString() + " - " + title;
                        file.Properties.System.Title.Value = loopIteration.ToString() + " - " + title;
                        File.Move(file.Path, folder + loopIteration.ToString() + " - " + title + ".mp3");
                    }
                    break;
            }
        }
    }
}
