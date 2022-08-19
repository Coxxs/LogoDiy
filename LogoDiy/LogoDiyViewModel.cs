using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Forms;

namespace LogoDiy
{
	public class LogoDiyViewModel
	{
		public enum FileExtension
		{
			[EnumDisplay(".jpg")]
			JPG = 255216,
			[EnumDisplay(".gif")]
			GIF = 7173,
			[EnumDisplay(".png")]
			PNG = 13780,
			[EnumDisplay(".bmp")]
			BMP = 19778,
			[EnumDisplay(".bmp")]
			BMP2 = 6677,
			[EnumDisplay(".jpeg")]
			JPEG = 65496
		}

		private enum ButtonStyle
		{
			JPG = 1,
			TGA = 2,
			PCX = 4,
			GIF = 8,
			BMP = 0x10,
			PNG = 0x20
		}

		private static LogoDiyViewModel _instance;

		private bool _IsShowLodingIco = true;

		private bool vidibalyLodingIco;

		private string _SupportingFormat = "jpg、jepg、bmp";

		private double _ImageHeight = 140.0;

		private double _ImageWidth = 224.0;

		private bool _ShowWarning;

		private string _ShowWarnInfo;

		private bool _ShowSuccessTip;

		private string showSuccessText = "";

		private BitmapImage imageSource;

		private bool _UIIsEnable = true;

		private bool funEnable = true;

		private long diskFreeSpace = 54525952L;

		private bool canRecovery;

		private string name;

		private int defaultHeight = 1080;

		private int defaultWidth = 1920;

		private string imagePath1;

		private string defaultpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images/pc.png");

		private string fileExtension = "";

		private string filter1 = "*.png,*.jpg,*.bmp";

		private string filter2 = "*.png;*.jpg;*.bmp";

		public static LogoDiyViewModel Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LogoDiyViewModel();
				}
				return _instance;
			}
		}

		public bool IsShowLodingIco
		{
			get
			{
				return _IsShowLodingIco;
			}
			set
			{
				_IsShowLodingIco = value;
				// RaisePropertyChanged(() => IsShowLodingIco);
			}
		}

		public bool VidibalyLodingIco
		{
			get
			{
				return vidibalyLodingIco;
			}
			set
			{
				vidibalyLodingIco = value;
				// RaisePropertyChanged(() => VidibalyLodingIco);
			}
		}

		public string Filter
		{
			get
			{
				return filter1;
			}
		}

		public string SupportingFormat
		{
			get
			{
				return _SupportingFormat;
			}
			set
			{
				_SupportingFormat = value;
				// RaisePropertyChanged(() => SupportingFormat);
			}
		}

		public double ImageHeight
		{
			get
			{
				return _ImageHeight;
			}
			set
			{
				_ImageHeight = value;
				// RaisePropertyChanged(() => ImageHeight);
			}
		}

		public double ImageWidth
		{
			get
			{
				return _ImageWidth;
			}
			set
			{
				_ImageWidth = value;
				// RaisePropertyChanged(() => ImageWidth);
			}
		}

		public bool ShowWarning
		{
			get
			{
				return _ShowWarning;
			}
			set
			{
				_ShowWarning = value;
				// RaisePropertyChanged(() => ShowWarning);
			}
		}

		public string ShowWarnInfo
		{
			get
			{
				return _ShowWarnInfo;
			}
			set
			{
				_ShowWarnInfo = value;
				// RaisePropertyChanged(() => ShowWarnInfo);
			}
		}

		public bool ShowSuccessTip
		{
			get
			{
				return _ShowSuccessTip;
			}
			set
			{
				_ShowSuccessTip = value;
				// RaisePropertyChanged(() => ShowSuccessTip);
			}
		}

		public string ShowSuccessText
		{
			get
			{
				return showSuccessText;
			}
			set
			{
				showSuccessText = value;
				// RaisePropertyChanged(() => ShowSuccessText);
			}
		}

		public BitmapImage ImageSource
		{
			get
			{
				return imageSource;
			}
			set
			{
				imageSource = value;
				// RaisePropertyChanged(() => ImageSource);
			}
		}

		public bool UIIsEnable
		{
			get
			{
				return _UIIsEnable;
			}
			set
			{
				_UIIsEnable = value;
				// RaisePropertyChanged(() => UIIsEnable);
			}
		}

		public bool FunEnable
		{
			get
			{
				return funEnable;
			}
			set
			{
				funEnable = value;
				// RaisePropertyChanged(() => FunEnable);
			}
		}

		public long DiskFreeSpace => diskFreeSpace;

		public bool CanRecovery
		{
			get
			{
				return canRecovery;
			}
			set
			{
				canRecovery = value;
				// RaisePropertyChanged(() => CanRecovery);
			}
		}

		public int DefaultHeight
		{
			get
			{
				return defaultHeight;
			}
			set
			{
				value = defaultHeight;
				// RaisePropertyChanged(() => DefaultHeight);
			}
		}

		public int DefaultWidth
		{
			get
			{
				return defaultWidth;
			}
			set
			{
				defaultWidth = value;
				// RaisePropertyChanged(() => DefaultWidth);
			}
		}

		public string ImagePath1
		{
			get
			{
				return imagePath1;
			}
			set
			{
				imagePath1 = value;
				// RaisePropertyChanged(() => ImagePath1);
			}
		}

		private LogoDiyViewModel()
		{
		}

		public void CreateViewData()
		{
			ImageHeight = 140.0;
			ImageWidth = 224.0;
			ShowSuccessTip = false;
			ShowWarning = false;
			GetLogoInfo();
			VidibalyLodingIco = GetLoadingCircle();
			if (VidibalyLodingIco)
			{
				GetShowLodingIco();
			}
		}

		private bool GetLoadingCircle()
		{
			int build = Environment.OSVersion.Version.Build;
			LogoDiy.LogHelper.Info($"ostype = {build}");
			if (build > 14393)
			{
				int num = DiyHelper.IsEditionCircle();
				LogoDiy.LogHelper.Info($"IsEditionCircle = {num}");
				if (num != 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		private void GetShowLodingIco()
		{
			IntPtr ptr = default(IntPtr);
			try
			{
				DiyHelper.Wow64DisableWow64FsRedirection(ref ptr);
				string text = "bcdedit /enum all";
				Process process = new Process();
				process.StartInfo.FileName = "cmd.exe";
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardInput = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				process.StandardInput.WriteLine(text + "&exit");
				process.StandardInput.AutoFlush = true;
				StreamReader standardOutput = process.StandardOutput;
				string text2 = standardOutput.ReadLine();
				bool flag = false;
				while (!standardOutput.EndOfStream)
				{
					text2 = standardOutput.ReadLine();
					if ((text2.Contains("标识符") || text2.Contains("identifier")) && text2.Contains("{globalsettings}"))
					{
						flag = true;
					}
					if (flag && text2.Contains("bootuxdisabled"))
					{
						Console.WriteLine("gobalsettings: " + text2);
						string text3 = text2.Replace("bootuxdisabled", "").Trim();
						IsShowLodingIco = text3.Contains("No");
						LogoDiy.LogHelper.Info($"{text2}; 加载系统图标 = {IsShowLodingIco}");
						flag = false;
					}
				}
				process.WaitForExit();
				process.Close();
			}
			catch (Exception)
			{
			}
			finally
			{
				DiyHelper.Wow64RevertWow64FsRedirection(ptr);
			}
		}

		private long GetHardDiskFreeSpace(string str_HardDiskName)
		{
			try
			{
				str_HardDiskName += ":\\";
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					Console.WriteLine($"{driveInfo.Name}");
					if (driveInfo.Name == str_HardDiskName)
					{
						return driveInfo.TotalFreeSpace;
					}
				}
			}
			catch (Exception)
			{
			}
			return 0L;
		}

		private void GetDiskFree(long freeM)
		{
			try
			{
				long num = 33554432L;
				diskFreeSpace = freeM - num;
				LogoDiy.LogHelper.Info($"diskFreeSpace = {diskFreeSpace}; freeTotal = {freeM};  _32M = {num} ");
			}
			catch (Exception ex)
			{
				LogoDiy.LogHelper.Error(ex);
			}
		}

		private bool IsSizeExceed(string path)
		{
			Image image = null;
			try
			{
				Path.GetExtension(path);
				image = Image.FromFile(path);
				double num = image.Width;
				double num2 = image.Height;
				if (num > (double)DefaultWidth || num2 > (double)DefaultHeight)
				{
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				image?.Dispose();
			}
		}

		private bool SetImageSize(string path)
		{
			Image image = null;
			try
			{
				Path.GetExtension(path);
				image = Image.FromFile(path);
				double num = image.Width;
				double num2 = image.Height;
				LogoDiy.LogHelper.Info($"img h = {num2}; w = {num}");
				ImageHeight = 140.0 * num2 / (double)defaultHeight;
				ImageWidth = 224.0 * num / (double)defaultWidth;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				image?.Dispose();
			}
		}

		private void SetImagePath(bool defaultImage)
		{
			string tempPath = Path.GetTempPath();
			try
			{
				string text = ChangeEFIDisk(mount: true);
				long hardDiskFreeSpace = GetHardDiskFreeSpace(text);
				string path = Path.Combine(text + ":", "EFI\\Lenovo\\Logo");
				if (!defaultImage && Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					if (files.Length != 0)
					{
						string text2 = files[0];
						tempPath = Path.Combine(tempPath, Path.GetFileName(text2));
						File.Copy(text2, tempPath, overwrite: true);
						SetImageSize(tempPath);
						GetBitmapImage(tempPath);
					}
				}
				else
				{
					GetBitmapImage(defaultpath);
				}
				GetDiskFree(hardDiskFreeSpace);
			}
			catch (Exception ex)
			{
				LogoDiy.LogHelper.Info($"SetImagePath error {ex.Message}");
			}
			finally
			{
				ChangeEFIDisk(mount: false);
			}
		}

		public void ToRecovery()
		{
			try
			{
				name = ChangeEFIDisk(mount: true);
				string text = "";
				text = Path.Combine(name + ":\\", "EFI\\Lenovo\\Logo");
				DeleteOtherDirectory(text);
			}
			catch (Exception ex)
			{
				LogoDiy.LogHelper.Error(ex);
			}
			finally
			{
				ChangeEFIDisk(mount: false);
			}
			int num = DiyHelper.SetLogoDIYInfo(0);
			LogoDiy.LogHelper.Info($"set logoinfo ret = {num}");
			if (num == 0)
			{
				LogoDiy.LogHelper.Info($"set logoinfo error: ret = {num}");
				return;
			}
			ShowSuccessText = "*默认设置恢复成功";
			ShowSuccessTip = true;
			FunEnable = false;
		}

		private void GetLogoInfo()
		{
			uint format = 0u;
			byte enable = 0;
			int logoDIYInfo = DiyHelper.GetLogoDIYInfo(ref enable, ref format, ref defaultHeight, ref defaultWidth);

			LogoDiy.LogHelper.Info($"ret1 = {logoDIYInfo}; Height = {defaultHeight}; Width = {defaultWidth}");
			if (logoDIYInfo != 0)
			{
				ChangeSupportingFormat(format);
				if (defaultHeight < 0 || defaultWidth < 0)
				{
					LogoDiy.LogHelper.Info("get height/width error, set default 1920*1080");
					DefaultWidth = 1920;
					DefaultHeight = 1080;
				}
				LogoDiy.LogHelper.Info($"get logoinfo -> enable = {enable}; type = {format}; ");
				bool flag = enable == 0;
				SetImagePath(flag);
				CanRecovery = !flag;
				FunEnable = !flag;
				UIIsEnable = true;
			}
			else
			{
				GetBitmapImage(defaultpath);
				LogoDiy.LogHelper.Info("get logo_info error:");
				UIIsEnable = false;
				FunEnable = false;
				CanRecovery = false;
			}
		}

		public void SaveLogoClick()
		{
			try
			{
				Console.WriteLine("SaveLogoClick");
				int num = DiyHelper.SetLogoDIYInfo(1);
				LogoDiy.LogHelper.Info($"set logoinfo ret = {num}");
				if (num == 0)
				{
					ShowSuccessTip = false;
					LogoDiy.LogHelper.Info($"set logoinfo error: ret = {num}");
					return;
				}
				LogoDiy.LogHelper.Info($"set logoinfo success:  ret = {num}");
				name = ChangeEFIDisk(mount: true);
				string text = "";
				try
				{
					string extension = Path.GetExtension(ImagePath1);
					if (!string.IsNullOrWhiteSpace(fileExtension) && extension != fileExtension)
					{
						extension = fileExtension;
					}
					text = Path.Combine(name + ":\\", "EFI\\Lenovo\\Logo", $"mylogo_{DefaultWidth}x{DefaultHeight}" + extension);
					LogoDiy.LogHelper.Info($"path = {text}");
					DeleteOtherFile(text);
					LogoDiy.LogHelper.Info($"source path = {ImagePath1}; dest path = {text}");
					File.Copy(ImagePath1, text);
				}
				catch (Exception ex)
				{
					LogoDiy.LogHelper.Info("copy file error:" + ex.Message);
					return;
				}
				if (DiyHelper.SetLogDIYCRC(text) > 0)
				{
					LogoDiy.LogHelper.Info("Set CRC success");
					ChangeEFIDisk(mount: false);
					ShowSuccessText = "*设置成功，请重启电脑查看效果";
					ShowSuccessTip = true;
				}
				else
				{
					LogoDiy.LogHelper.Info("Set CRC ERROR");
					ShowSuccessTip = false;
				}
			}
			catch (Exception ex2)
			{
				LogoDiy.LogHelper.Error(ex2);
				ShowSuccessTip = false;
			}
			finally
			{
				CanRecovery = true;
				ChangeEFIDisk(mount: false);
			}
		}

		private void DeleteOtherDirectory(string path)
		{
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
		}

		private void DeleteOtherFile(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			LogoDiy.LogHelper.Info($"delete directory {directoryName}");
			if (Directory.Exists(directoryName))
			{
				LogoDiy.LogHelper.Info("Delete directory error, to delete file");
				string[] files = Directory.GetFiles(directoryName);
				foreach (string text in files)
				{
					LogoDiy.LogHelper.Info($"delete other file {text}");
					File.Delete(text);
				}
			}
			else
			{
				LogoDiy.LogHelper.Info($"Create new directory {directoryName}");
				Directory.CreateDirectory(directoryName);
			}
		}

		private string GetDiskName()
		{
			string[] logicalDrives = Directory.GetLogicalDrives();
			for (int i = 65; i < 91; i++)
			{
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				byte[] bytes = new byte[1]
				{
					(byte)i
				};
				string @string = aSCIIEncoding.GetString(bytes);
				if (!logicalDrives.Contains(@string))
				{
					return @string;
				}
			}
			return "";
		}

		private string ChangeEFIDisk(bool mount)
		{
			string diskName = GetDiskName();
			try
			{
				string arg = (mount ? "/s" : "/d");
				string text = $"mountvol {diskName}: {arg}";
				LogoDiy.LogHelper.Info(text);
				Process process = new Process();
				process.StartInfo.FileName = "cmd.exe";
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardInput = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				process.StandardInput.WriteLine(text + "&exit");
				process.StandardInput.AutoFlush = true;
				process.WaitForExit();
				process.Close();
				return diskName;
			}
			catch (Exception ex)
			{
				LogoDiy.LogHelper.Error(ex);
				return diskName;
			}
		}

		public bool ChangeLodingIco(bool isShow)
		{
			IntPtr ptr = default(IntPtr);
			try
			{
				DiyHelper.Wow64DisableWow64FsRedirection(ref ptr);
				string text = "";
				text = ((!isShow) ? "bcdedit.exe -set {globalsettings} bootuxdisabled on" : "bcdedit.exe -set {globalsettings} bootuxdisabled off");
				Console.WriteLine(text);
				LogoDiy.LogHelper.Info(text);
				Process process = new Process();
				process.StartInfo.FileName = "cmd.exe";
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardInput = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				process.StandardInput.WriteLine(text + "&exit");
				process.StandardInput.AutoFlush = true;
				process.WaitForExit();
				process.Close();
				return true;
			}
			catch (Exception ex)
			{
				LogoDiy.LogHelper.Error(ex);
				return false;
			}
			finally
			{
				DiyHelper.Wow64RevertWow64FsRedirection(ptr);
			}
		}

		public bool ImageCheck(string path)
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			string empty = string.Empty;
			bool flag = false;
			flag = IsImage(path);
			if (!flag)
			{
				return flag;
			}
			try
			{
				Path.GetExtension(path);
				empty += binaryReader.ReadByte();
				empty += binaryReader.ReadByte();
				try
				{
					FileExtension fileExtension = (FileExtension)Enum.Parse(typeof(FileExtension), empty);
					switch (fileExtension)
					{
						case FileExtension.BMP2:
						case FileExtension.PNG:
						case FileExtension.BMP:
						case FileExtension.JPEG:
						case FileExtension.JPG:
							this.fileExtension = fileExtension.Display();
							flag = true;
							return flag;
						default:
							flag = false;
							return flag;
					}
				}
				catch (Exception ex)
				{
					LogoDiy.LogHelper.Error(ex);
					return flag;
				}
			}
			catch (Exception ex2)
			{
				LogoDiy.LogHelper.Error(ex2);
				return flag;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					binaryReader.Close();
				}
			}
		}

		private bool IsImage(string path)
		{
			Image image = null;
			try
			{
				string text = Path.GetExtension(path).ToLower();
				if (!(text == ".tga") && !(text == ".pcx"))
				{
					image = Image.FromFile(path);
					image.Dispose();
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				image?.Dispose();
			}
		}

		public void SelectedImageClick()
		{
			ShowSuccessTip = false;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.Title = "请选择图片";
			openFileDialog.Filter = $"图片文件({filter1})|{filter2}";
			var res = openFileDialog.ShowDialog();
			if (res == DialogResult.OK)
			{
				CanRecovery = false;
				FunEnable = false;
				string fileName = openFileDialog.FileName;
				FileInfo fileInfo = new FileInfo(fileName);
				if (!ImageCheck(fileName))
				{
					ShowWarning = true;
					ShowWarnInfo = "*当前选择的文件不是图片，请重试！";
					return;
				}
				if (fileInfo.Length > DiskFreeSpace)
				{
					int num = (int)(DiskFreeSpace / 1024 / 1024);
					ShowWarnInfo = $"*图片不得超过{num}MB，请重新上传！";
					ShowWarning = true;
					return;
				}
				if (IsSizeExceed(fileName))
				{
					ShowWarnInfo = "*图片超出最大分辨率，请重新上传！";
					ShowWarning = true;
					return;
				}
				GetBitmapImage(fileName);
				SetImageSize(fileName);
				FunEnable = true;
				ShowWarning = false;
				LogoDiy.LogHelper.Info($"界面图片大小>>height = {ImageHeight}; width = {ImageWidth};");
			}
		}

		public void GetBitmapImage(string imagePath)
		{
			try
			{
				imagePath1 = imagePath;
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.UriSource = new Uri(imagePath);
				bitmapImage.EndInit();
				ImageSource = bitmapImage.Clone();
				bitmapImage.Freeze();
			}
			catch (Exception ex)
			{
				LogoDiy.LogHelper.Error(ex);
			}
		}

		private void ChangeSupportingFormat(uint format)
		{
			filter1 = "";
			filter2 = "";
			if ((1u & format) != 0)
			{
				filter1 += "*.jpg,";
				filter2 += "*.jpg;";
			}
			if ((0x10u & format) != 0)
			{
				filter1 += "*.bmp,";
				filter2 += "*.bmp;";
			}
			if ((0x20u & format) != 0)
			{
				filter1 += "*.png,";
				filter2 += "*.png;";
			}
			filter1 = filter1.Substring(0, filter1.Length - 1);
			filter2 = filter2.Substring(0, filter2.Length - 1);
			SupportingFormat = filter2;
		}
	}
}
