using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using Ionic.Zip;
using System.IO.Compression;

namespace ConsoleApp13
{
	class Program
	{
		static void Main(string[] args)
		{
			NISFileManager pAAFileManager = new NISFileManager();
			Console.ReadLine();
		}
	}
	class NISLog
	{
		public NISLog()
		{
			Console.Write("Введите путь к файлу: ");
			string pFile = Console.ReadLine();
			Console.Write("Введите имя файла: ");
			string nameFile = Console.ReadLine();
			Console.Write("Введите текущее время: ");
			string time = Console.ReadLine();
			if (pFile != null && nameFile != null && time != null)
			{ 
				try
				{
						string pFile1 = pFile + @"\" + nameFile;
						using (StreamWriter sw = new StreamWriter(pFile1, false, System.Text.Encoding.Default))
						{
							sw.WriteLine("Путь к файлу: " + pFile);
							sw.WriteLine("Имя файла: " + nameFile);
							sw.WriteLine("Время: " + time);
						}
							Console.WriteLine($"Путь к файлу: {pFile}, имя файла: {nameFile}, время: {time}");
				}
				catch(Exception ex)
				{
						throw new Exception(ex.Message);
				}
			}
		}
	}
	class NISDiskInfo
	{
		public NISDiskInfo()
		{
			DriveInfo[] allDrives = DriveInfo.GetDrives();
			foreach (DriveInfo d in allDrives)
			{ 
				Console.WriteLine("Имя диска {0}", d.Name);
				Console.WriteLine("  Тип диска: {0}", d.DriveType);
				if (d.IsReady == true)
				{
					Console.WriteLine("  Метка тома: {0}", d.VolumeLabel);
					Console.WriteLine("  Файловая система: {0}", d.DriveFormat);
					Console.WriteLine(
						"  Доступное место для текущего пользователя:{0, 15} bytes",
						d.AvailableFreeSpace);
					Console.WriteLine(
						"  Общая доступная площадь:          {0, 15} bytes",
						d.TotalFreeSpace);
					Console.WriteLine(
						"  Общий размер диска:            {0, 15} bytes ",
						d.TotalSize);
				}
			}
		}            
	}
	class NISFileInfo
	{
		public NISFileInfo()
		{
			Console.Write("Введите путь к файлу о котором хотите узнать информацию: ");
			string path = Console.ReadLine();
			FileInfo fileInf = new FileInfo(path);
			if(fileInf.Exists)
			{
				Console.WriteLine("Полный путь: {0}", fileInf.DirectoryName);
				Console.WriteLine("Размер: {0}", fileInf.Length);
				Console.WriteLine("Расширение: {0}", fileInf.Extension);
				Console.WriteLine("Имя: {0}", fileInf.Name);
				Console.WriteLine("Время создания: {0}", fileInf.CreationTime);
			}
		}
	}
	class NISDirInfo
	{
		public NISDirInfo()
		{
			Console.Write("Введите директорию о которой хотите узнать информацию: ");
			string path = Console.ReadLine();
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if(directoryInfo.Exists)
			{
				Console.WriteLine("Количество файлов: {0}", directoryInfo.GetFiles().Length);
				Console.WriteLine("Время создания: {0}", directoryInfo.CreationTime);
				Console.WriteLine("Количество поддиректориев: {0}", directoryInfo.GetDirectories().Length);
				Console.WriteLine("Количество родительских директориев: {0}", directoryInfo.Parent.GetDirectories().Length);
			}
		}
	}
	class NISFileManager
	{
		public NISFileManager()
		{
			Console.Write("Введите диск о которой хотите узнать информацию: ");
			string path = Console.ReadLine() + @":\";
			DriveInfo[] allDrives = DriveInfo.GetDrives();
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			foreach (DriveInfo driveInfo in allDrives)
			{
				if (directoryInfo.Exists && driveInfo.IsReady && path == driveInfo.Name)
				{
					Console.WriteLine("Количество файлов: {0}", directoryInfo.GetFiles().Length);
					Console.WriteLine("Время создания: {0}", directoryInfo.CreationTime);
					Console.Write("Введите имя создаваемого каталога: ");
					string createDir = Console.ReadLine();
					if(createDir != null)
					{ 
						directoryInfo.CreateSubdirectory(createDir);
						DirectoryInfo fileInfo = new DirectoryInfo(path+@"\"+createDir);
						if(fileInfo.Exists)
						{
							Console.Write("Введите имя создаваемого файла: ");
							string createFile = Console.ReadLine();
							StreamWriter file = new StreamWriter(path + @"\" + createDir + @"\" + createFile);
							file.WriteLine("Имя файла: " + createFile);
							file.WriteLine("Путь к файлу: " + path + @"\" + createDir);
							file.Close();
							File.Copy(path + @"\" + createDir + @"\" + createFile, path + @"\" + createDir + @"\" + "Copy"+createFile);
							File.Delete(path + @"\" + createDir + @"\" + createFile);
							file.Close();
						}
						directoryInfo.CreateSubdirectory("NISFiles");
						DirectoryInfo fileInfo2 = new DirectoryInfo(path + @"\" + "NISFiles");
						if(fileInfo2.Exists)
						{ 
							Console.WriteLine("Введите путь откуда надо скопировать файлы: ");
							string path2 = Console.ReadLine();
							Console.WriteLine("Введите расширение файлов для копирования: ");
							string rash = Console.ReadLine();
							string path3 = path + @"\" + "NISFiles";
							try
							{
								DirectoryInfo directoryInfo2 = new DirectoryInfo(path2);
								DirectoryInfo directoryInfo3 = new DirectoryInfo(path3);
								foreach (FileInfo fileInfo3 in directoryInfo2.GetFiles("*." + rash))
								{
									File.Copy(fileInfo3.FullName, path3 + "\\" + fileInfo3.Name, true);
								}
								directoryInfo3.MoveTo(path + @"\" + createDir + @"\" + "NewDir" + @"\");
								
								using (ZipFile zip = new ZipFile())
								{
									zip.AddDirectory(path + @"\" + createDir + @"/");
									zip.Save(path + @"\" + createDir + @"\NewRar.rar");
									GZipStream stream = new GZipStream(new FileStream(path, FileMode.Open, FileAccess.ReadWrite), CompressionMode.Decompress);

								}
							}
							catch(Exception ex)
							{
								Console.WriteLine(ex.Message);
							}
						}

					}
				}

			}
			foreach(DriveInfo driveInfo in allDrives)
			{
				
			}

		}

	}

}
	
