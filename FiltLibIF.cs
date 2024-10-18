using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace OMRON_IFZ_Viewer
{
	public class FiltLibIF
	{
		private const int IMGHEAD_SIZE = 80;

		public const string SaveImgBfz = "bfz";

		public const string SaveImgBitmap = "bmp";

		public const string SaveImgJpeg = "jpg";

		public const string SaveImgTiff = "tif";

		public FiltLibIF()
		{
		}

		[DllImport("FZ-ImageProcess.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern unsafe int BayerToRGBImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, int arrayType, int maskSize, byte* p);

		[DllImport("FZ-ImageProcess.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern unsafe int BayerToRGBImage2(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, int arrayType, byte* p);

        [DllImport("FZ-ImageProcess.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern unsafe int MakeGrayImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, int rate, byte* p);

        [DllImport("FZ-ImageProcess.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern unsafe int RGB24ToImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, byte* p);
       
		[DllImport("FZ-ImageProcess.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern unsafe int ImageToBMP2(byte* ip, int format, int width, int height, byte[] op);

        [DllImport("FZ-ImageProcess.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern int ImageToBMPSize2(int format, int width, int height);

		[DllImport("FZ-ImageProcess.dll", CharSet = CharSet.None, ExactSpelling = false)]
		private static extern unsafe int YUV422ToImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, byte* p);

		public static int ByrtoBmp(BayerMaster byrMaster, out Bitmap mybmp, int img_no)
		{
			Bitmap bitmap;
			int num;
			mybmp = null;
			
			if (img_no < 0)
			{
				int byrArray = 0;
				int byrArray1 = 0;
				for (int i = 0; i < byrMaster.camno; i++)
				{
					if (byrArray < byrMaster.ByrArray[i].xsize)
					{
						byrArray = byrMaster.ByrArray[i].xsize;
					}
					byrArray1 += byrMaster.ByrArray[i].ysize;
				}
				mybmp = new Bitmap(byrArray, byrArray1);
				using (Graphics graphic = Graphics.FromImage(mybmp))
				{
					int height = 0;
					for (int j = 0; j < byrMaster.camno; j++)
					{
						ByrtoBmp(byrMaster.ByrArray[j], out bitmap);
						graphic.DrawImage(bitmap, 0, height, bitmap.Width, bitmap.Height);
						height += bitmap.Height;
					}
				}
			}
			else if (img_no < byrMaster.camno)
			{
				if (byrMaster.ByrArray[img_no].height == 0 || byrMaster.ByrArray[img_no].width == 0)
				{
					return -1;
				}
				num = ByrtoBmp(byrMaster.ByrArray[img_no], out mybmp);
				return num;
			}
			return -1;
		}
        public static int ByrtoBmp2(BayerData byrdata, out Bitmap mybmp)
        {
            unsafe
            {
                byte* numPointer;
                int num;
                int rGBImage = 0;
                int num1 = 0;
                int num2 = 0;
                byte[] numArray = byrdata.data;
                int num3 = 0;
                mybmp = new Bitmap(1, 1);
                try
                {

                    int num4 = byrdata.format;
                    switch (num4)
                    {
                        case 0:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 0;
                                break;
                            }
                        case 1:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 1;
                                break;
                            }
                        case 2:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 2;
                                break;
                            }
                        case 3:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 3;
                                break;
                            }

                        case 10:
                        case 11:
                        case 100:
                            {
                                num1 = byrdata.xsize * byrdata.ysize + 80;
                                num2 = 1;
                                break;
                            }

                        case 101:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 0;
                                break;
                            }
                        case 102:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 1;
                                break;
                            }
                        case 103:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 2;
                                break;
                            }
                        case 104:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                num3 = 3;
                                break;
                            }
                        case 108:
                        case 111:
                            {
                                num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
                                num2 = 0;
                                break;
                            }
                    }
                    try
                    {
                        byte[] numArray1 = new byte[num1];
                        byte[] numArray2 = numArray1;
                        if (numArray1 == null || (int)numArray2.Length == 0)
                        {
                            numPointer = null;

                        }
                        else
                        {
                            fixed (byte* numPointer2 = &numArray2[0])
                            {
                                numPointer = numPointer2;
                            };

                        }
                        int num5 = 0;
                        int num6 = byrdata.xsize;
                        int num7 = byrdata.ysize;
                        int num8 = byrdata.x0;
                        int num9 = byrdata.y0;
                        int num10 = num8 + byrdata.width - 1;
                        int num11 = num9 + byrdata.height - 1;
                        switch (num2)
                        {
                            case 0:
                                {
                                    num5 = 2;
                                    int num12 = byrdata.format;
                                    switch (num12)
                                    {
                                        case 0:
                                        case 1:
                                        case 2:
                                        case 3:
                                            {
                                                rGBImage = BayerToRGBImage(numArray, num6, num7, num8, num9, num10, num11, num3, 0, numPointer);
                                                break;
                                            }

                                        case 101:
                                        case 102:
                                        case 103:
                                        case 104:
                                            {
                                                rGBImage = BayerToRGBImage2(numArray, num6, num7, num8, num9, num10, num11, num3, numPointer);
                                                break;
                                            }
                                        case 105:
                                        case 106:
                                        case 107:
                                        case 109:
                                        case 110:
                                            {
                                                rGBImage = BayerToRGBImage(numArray, num6, num7, num8, num9, num10, num11, num3, 0, numPointer);
                                                break;
                                            }
                                        case 108:
                                            {
                                                rGBImage = YUV422ToImage(numArray, num6, num7, num8, num9, num10, num11, numPointer);
                                                break;
                                            }
                                        case 111:
                                            {
                                                rGBImage = RGB24ToImage(numArray, num6, num7, num8, num9, num10, num11, numPointer);
                                                break;
                                            }
                                        default:
                                            {
                                                goto case 110;
                                            }
                                    }
                                    break;

                                }
                            case 1:
                                {
                                    num5 = 1;
                                    rGBImage = MakeGrayImage(numArray, num6, num7, num8, num9, num10, num11, byrdata.format % 2, numPointer);
                                    break;
                                }
                        }
                        if (rGBImage == 0)
                        {
                            byte[] numArray3 = new byte[ImageToBMPSize2(num5, num6, num7)];
                            ImageToBMP2(numPointer + 80, num5, num6, num7, numArray3);
                            MemoryStream memoryStream = new MemoryStream(numArray3);
                            mybmp = new Bitmap(memoryStream);
                            memoryStream.Close();
                        }
                    }
                    finally
                    {
                        numPointer = null;
                    }
                    return rGBImage;
                }
                catch
                {
                    num = -1;
                }
                return num;
            }
        }

		public static int ByrtoBmp(BayerData byrdata, out Bitmap mybmp)
		{
			unsafe
			{
				byte* numPointer;
				int num;
				int rGBImage = 0;
				int num1 = 0;
				int num2 = 0;
				byte[] numArray = byrdata.data;
				int num3 = 0;
				mybmp = new Bitmap(1, 1);
				try
				{
					int num4 = byrdata.format;
					switch (num4)
					{
						case 0:
							{
								num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
								num2 = 0;
								num3 = 0;
								break;
							}
						case 1:
							{
								num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
								num2 = 0;
								num3 = 1;
								break;
							}
						case 2:
							{
								num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
								num2 = 0;
								num3 = 2;
								break;
							}
						case 3:
							{
								num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
								num2 = 0;
								num3 = 3;
								break;
							}
						default:
							{
								switch (num4)
								{
									case 10:
									case 11:
									case 100:
										{

											num1 = byrdata.xsize * byrdata.ysize + 80;
											num2 = 1;
											break;
										}

									case 101:
										{
											num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
											num2 = 0;
											num3 = 0;
											break;
										}
									case 102:
										{
											num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
											num2 = 0;
											num3 = 1;
											break;
										}
									case 103:
										{
											num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
											num2 = 0;
											num3 = 2;
											break;
										}
									case 104:
										{
											num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
											num2 = 0;
											num3 = 3;
											break;
										}
									case 108:
									case 111:
										{
											num1 = byrdata.xsize * byrdata.ysize * 3 + 80;
											num2 = 0;
											break;
										}
								}
								break;
							}


					}
					try
					{
						byte[] numArray1 = new byte[num1];
						byte[] numArray2 = numArray1;
						if (numArray1 == null || (int)numArray2.Length == 0)
						{
							numPointer = null;
						}
						else
						{
							fixed (byte* numPointer2 = &numArray2[0])
							{
								numPointer = numPointer2;
							};
						}
						int num5 = 0;
						int num6 = byrdata.xsize;
						int num7 = byrdata.ysize;
						int num8 = byrdata.x0;
						int num9 = byrdata.y0;
						int num10 = num8 + byrdata.width - 1;
						int num11 = num9 + byrdata.height - 1;
						switch (num2)
						{
							case 0:
								{
									num5 = 2;
									int num12 = byrdata.format;
									switch (num12)
									{
										case 0:
										case 1:
										case 2:
										case 3:
											{
												rGBImage = BayerToRGBImage(numArray, num6, num7, num8, num9, num10, num11, num3, 0, numPointer);
												break;
											}
										case 101:
										case 102:
										case 103:
										case 104:
											{
												rGBImage = BayerToRGBImage2(numArray, num6, num7, num8, num9, num10, num11, num3, numPointer);
												break;
											}
										case 105:
										case 106:
										case 107:
										case 109:
										case 110:
											{
												rGBImage = BayerToRGBImage(numArray, num6, num7, num8, num9, num10, num11, num3, 0, numPointer);
												break;
											}
										case 108:
											{
												rGBImage = YUV422ToImage(numArray, num6, num7, num8, num9, num10, num11, numPointer);
												break;
											}
										case 111:
											{
												rGBImage = RGB24ToImage(numArray, num6, num7, num8, num9, num10, num11, numPointer);
												break;
											}
										default:
											{
												goto case 110;
											}
									}
									break;
								}

							case 1:
								{
									num5 = 1;
									rGBImage = MakeGrayImage(numArray, num6, num7, num8, num9, num10, num11, byrdata.format % 2, numPointer);
									break;
								}
						}
						if (rGBImage == 0)
						{
							byte[] numArray3 = new byte[ImageToBMPSize2(num5, num6, num7)];
							ImageToBMP2(numPointer + 80, num5, num6, num7, numArray3);
							if (rGBImage == 0)
							{
								MemoryStream memoryStream = new MemoryStream(numArray3);
								mybmp = new Bitmap(memoryStream);
								memoryStream.Close();
							}
						}
					}
					finally
					{
						numPointer = null;
					}
					return rGBImage;
				}
				catch
				{
					num = -1;
				}
				return num;
			}
		}
        public static bool CheckCaptures(string filename)
		{
			int num;
			bool flag = false;
			FileStream fileStream = null;
			int num1 = 0;
			ImageFileInfo imageFileInfo = GetImageFileInfo(filename);
			try
			{
				try
				{
					fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
					if (imageFileInfo.type != IMAGE_TYPE.BYR)
					{
						num1 = IfzGetCaptureNoMax(fileStream, out num);
					}
					else if (imageFileInfo.image_num > 0)
					{
						num1 = 1;
					}
					if (num1 == 1)
					{
						flag = true;
					}
				}
                catch (Exception ex)
                {
                    using (StreamWriter writer = new StreamWriter("D:\\IFZ Viewer log.txt", true))
                        writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff") + " : " + ex.Message);
                }
            }
			finally
			{
                if (fileStream != null)
				{
                    fileStream.Seek(0, SeekOrigin.Begin);
                   // fileStream.Close();
				}
			}
			return flag;
		}

		private static ImageCodecInfo GetEncoderInfo(string mineType)
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			for (int i = 0; i < (int)imageEncoders.Length; i++)
			{
				ImageCodecInfo imageCodecInfo = imageEncoders[i];
				if (imageCodecInfo.MimeType == mineType)
				{
					return imageCodecInfo;
				}
			}
			return null;
		}

		public static ImageFileInfo GetImageFileInfo(string filename)
		{
			ImageFileInfo imageFileInfo = new ImageFileInfo();
			byte[] numArray = new byte[16];
			FileStream fileStream = null;
			imageFileInfo.type = GetImageType(filename);
			IMAGE_TYPE mAGETYPE = imageFileInfo.type;
			if (mAGETYPE > IMAGE_TYPE.BYR)
			{
				if (mAGETYPE != IMAGE_TYPE.IFZ)
				{
					goto Label2;
				}
				try
				{
					fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
					fileStream.Read(numArray, 0, 4);
					fileStream.Read(numArray, 0, 4);
				}
				finally
				{
					if (fileStream != null)
					{
						//fileStream.Close();
                        fileStream.Seek(0, SeekOrigin.Begin);
                    }
				}
				imageFileInfo.image_num = BitConverter.ToInt32(numArray, 0);
				return imageFileInfo;
			}
			else
			{
				if (mAGETYPE == IMAGE_TYPE.BMITMAP)
				{
					imageFileInfo.image_num = 1;
					return imageFileInfo;
				}
				if (mAGETYPE != IMAGE_TYPE.BYR)
				{
					imageFileInfo.image_num = 0;
					return imageFileInfo;
				}
				try
				{
					fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
					fileStream.Read(numArray, 0, 4);
				}
				finally
				{
					if (fileStream != null)
					{
						//fileStream.Close();
                        fileStream.Seek(0, SeekOrigin.Begin);
                    }
				}
				imageFileInfo.image_num = BitConverter.ToInt32(numArray, 0);
				return imageFileInfo;
			}
			imageFileInfo.image_num = 1;
			return imageFileInfo;
		Label2:
			if (mAGETYPE == IMAGE_TYPE.JPG || mAGETYPE == IMAGE_TYPE.TIF)
			{
				imageFileInfo.image_num = 1;
				return imageFileInfo;
			}
			else
			{
				imageFileInfo.image_num = 0;
				return imageFileInfo;
			}
		}
        public static IMAGE_TYPE GetImageType(string filename)
        {
            IMAGE_TYPE mAGETYPE;
            byte[] numArray = new byte[16];
            FileStream fileStream = null;
            try
            {
                FileStream fileStream1 = new FileStream(filename, FileMode.Open, FileAccess.Read);
                fileStream = fileStream1;
                if (fileStream1 != null)
                {
                    fileStream.Read(numArray, 0, 4);
                    int num = BitConverter.ToInt32(numArray, 0);
                    if (num == -2)
                    {
                        return IMAGE_TYPE.IFZ;
                    }
                    if ((num & 65535) == 18761 || (num & 65535) == 19789)
                    {
                        return IMAGE_TYPE.TIF;
                    }
                    if ((num & 65535) == 19778)
                    {
                        return IMAGE_TYPE.BMITMAP;
                    }
                    if ((num & 65535) == 55551)
                    {
                        return IMAGE_TYPE.JPG;
                    }
                    if (num > 0 && num < 10)
                    {
                        return IMAGE_TYPE.BYR;
                    }
                    return IMAGE_TYPE.UN_KNOWN;
                }
                else
                {
                    mAGETYPE = IMAGE_TYPE.UN_KNOWN;
                }
            }
            finally
            {
               // fileStream.Close();
                fileStream.Seek(0, SeekOrigin.Begin);
            }
            return mAGETYPE;
        }

        public static ImageFileInfo GetImageFileInfo(Stream fileStream)
        {
            ImageFileInfo imageFileInfo = new ImageFileInfo();
            byte[] numArray = new byte[16];
            //FileStream fileStream = null;
            imageFileInfo.type = GetImageType(fileStream);
            IMAGE_TYPE mAGETYPE = imageFileInfo.type;
            if (mAGETYPE > IMAGE_TYPE.BYR)
            {
                if (mAGETYPE != IMAGE_TYPE.IFZ)
                {
                    goto Label2;
                }
                try
                {
                    //fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    fileStream.Read(numArray, 0, 4);
                    fileStream.Read(numArray, 0, 4);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        //fileStream.Close();
                    }
                }
                imageFileInfo.image_num = BitConverter.ToInt32(numArray, 0);
                return imageFileInfo;
            }
            else
            {
                if (mAGETYPE == IMAGE_TYPE.BMITMAP)
                {
                    imageFileInfo.image_num = 1;
                    return imageFileInfo;
                }
                if (mAGETYPE != IMAGE_TYPE.BYR)
                {
                    imageFileInfo.image_num = 0;
                    return imageFileInfo;
                }
                try
                {
                    //fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    fileStream.Read(numArray, 0, 4);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        //fileStream.Close();
                    }
                }
                imageFileInfo.image_num = BitConverter.ToInt32(numArray, 0);
                return imageFileInfo;
            }
            imageFileInfo.image_num = 1;
            return imageFileInfo;
        Label2:
            if (mAGETYPE == IMAGE_TYPE.JPG || mAGETYPE == IMAGE_TYPE.TIF)
            {
                imageFileInfo.image_num = 1;
                return imageFileInfo;
            }
            else
            {
                imageFileInfo.image_num = 0;
                return imageFileInfo;
            }
        }
        public static IMAGE_TYPE GetImageType(Stream fileStream)
        {
            IMAGE_TYPE mAGETYPE;
            byte[] numArray = new byte[16];
            //FileStream fileStream = null;
            try
            {
                //FileStream fileStream1 = new FileStream(filename, FileMode.Open, FileAccess.Read);
                //fileStream = fileStream1;
                if (fileStream != null)
                {
                    fileStream.Read(numArray, 0, 4);
                    int num = BitConverter.ToInt32(numArray, 0);
                    if (num == -2)
                    {
                        return IMAGE_TYPE.IFZ;
                    }
                    if ((num & 65535) == 18761 || (num & 65535) == 19789)
                    {
                        return IMAGE_TYPE.TIF;
                    }
                    if ((num & 65535) == 19778)
                    {
                        return IMAGE_TYPE.BMITMAP;
                    }
                    if ((num & 65535) == 55551)
                    {
                        return IMAGE_TYPE.JPG;
                    }
                    if (num > 0 && num < 10)
                    {
                        return IMAGE_TYPE.BYR;
                    }
                    return IMAGE_TYPE.UN_KNOWN;
                }
                else
                {
                    mAGETYPE = IMAGE_TYPE.UN_KNOWN;
                }
            }
            finally
            {
                //fileStream.Close();
                fileStream.Seek(0, SeekOrigin.Begin);
            }
            return mAGETYPE;
        }
        private static int IfzGetCaptureNoMax(Stream fStream, out int imageMax)
        {
            byte[] numArray = new byte[4];
            int i = 0;
            int num = 0;
            imageMax = 0;
            if (fStream == null)
            {
                return 0;
            }
            try
            {
                for (i = 0; i < 9999 && fStream.Position < fStream.Length; i++)
                {
                    fStream.Read(numArray, 0, 4);
                    fStream.Read(numArray, 0, 4);
                    int num1 = BitConverter.ToInt32(numArray, 0);
                    int num2 = 0;
                    for (int j = 0; j < 32; j++)
                    {
                        num2 += (num1 & 1);
                        num1 >>= 1;
                    }
                    int num3 = 0;
                    while (num3 < num2)
                    {
                        fStream.Read(numArray, 0, 4);
                        int num4 = BitConverter.ToInt32(numArray, 0);
                        int num5 = 1;
                        int num6 = 0;
                        if (num4 == 11 && (num4 & 1) == 1)
                        {
                            num5 = 2;
                        }
                        if (0 <= num4 && num4 <= 3)
                        {
                            num6 = 2;
                        }
                        fStream.Read(numArray, 0, 4);
                        BitConverter.ToInt32(numArray, 0);
                        fStream.Read(numArray, 0, 4);
                        BitConverter.ToInt32(numArray, 0);
                        fStream.Read(numArray, 0, 4);
                        BitConverter.ToInt32(numArray, 0);
                        fStream.Read(numArray, 0, 4);
                        BitConverter.ToInt32(numArray, 0);
                        fStream.Read(numArray, 0, 4);
                        int num7 = BitConverter.ToInt32(numArray, 0);
                        fStream.Read(numArray, 0, 4);
                        int num8 = BitConverter.ToInt32(numArray, 0);
                        fStream.Read(numArray, 0, 4);
                        num1 = BitConverter.ToInt32(numArray, 0);
                        int num9 = (num7 + num6) * (num8 + num6) / num5;
                        if (num4 == 108)
                        {
                            num9 *= 2;
                        }
                        else if (num4 == 111)
                        {
                            num9 *= 3;
                        }
                        if (num1 - 32 == num9)
                        {
                            fStream.Read(new byte[num9], 0, num9);
                            num++;
                            num3++;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch
            {
                num = 0;
            }
            fStream.Position = (long)0;
            imageMax = num;
            return i;
        }

		public static bool ImageFiletoBitmap(string FileName, out Bitmap Bmp, int imageno, int size_x, int size_y)
		{
			BayerMaster bayerMaster = new BayerMaster();
			Bitmap bitmap;
			ImageFileInfo imageFileInfo = GetImageFileInfo(FileName);
			Bmp = null;
			IMAGE_TYPE mAGETYPE = imageFileInfo.type;
			if (mAGETYPE <= IMAGE_TYPE.BYR)
			{
				if (mAGETYPE == IMAGE_TYPE.BMITMAP)
				{
					bitmap = new Bitmap(FileName);
				}
				else
				{
					if (mAGETYPE != IMAGE_TYPE.BYR)
					{
						return false;
					}
					bayerMaster = new BayerMaster();
					MakeByrData(FileName, ref bayerMaster);
					ByrtoBmp(bayerMaster, out bitmap, imageno);
				}
			}
			else if (mAGETYPE == IMAGE_TYPE.IFZ)
			{
				bayerMaster = new BayerMaster();
				MakeByrData(FileName, ref bayerMaster);
				ByrtoBmp(bayerMaster, out bitmap, imageno);
			}
			else if (mAGETYPE == IMAGE_TYPE.JPG)
			{
				bitmap = new Bitmap(FileName);
			}
			else
			{
				if (mAGETYPE != IMAGE_TYPE.TIF)
				{
					return false;
				}
				bitmap = new Bitmap(FileName);
			}
			using (bitmap)
			{
				if (size_x < 0)
				{
					size_x = bitmap.Width;
				}
				if (size_y < 0)
				{
					size_y = bitmap.Height;
				}
				Bmp = new Bitmap(bitmap, size_x, size_y);
				using (Graphics graphic = Graphics.FromImage(Bmp))
				{
					graphic.DrawImage(bitmap, 0, 0, size_x, size_y);
				}
			}
			bayerMaster.ByrArray = null;
			GC.Collect();
			return true;
		}
        [Serializable]
        public class TestException : ApplicationException
        {
            public TestException(string Message,Exception innerException) : base(Message, innerException) { }
            public TestException(string Message) : base(Message) { }
            public TestException() { }

            #region Serializeable Code
            public TestException(SerializationInfo info,StreamingContext context) : base(info, context) { }
            #endregion Serializeable Code
        }

        public static bool MakeByrData(string filename, ref BayerMaster byr)
		{
			bool flag = false;
			FileStream fileStream = null;
			ImageFileInfo imageFileInfo = GetImageFileInfo(filename);
			if (imageFileInfo.type == IMAGE_TYPE.UN_KNOWN)
			{ 
				throw new ApplicationException("Invalid IFZ file!");
			}
			try
			{
				try
				{
					fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
					flag = MakeByrData(fileStream, ref byr, imageFileInfo.type);
				}
                catch (Exception ex)
                {
					//MessageBox.Show(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff") + " : " + ex.Message);
                    //using (StreamWriter writer = new StreamWriter("D:\\IFZ Viewer log.txt", true))
                    //    writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff") + " : " + ex.Message);
                }
            }
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			return flag;
		}
        public static bool MakeByrDataFromStream(Stream fStream, ref BayerMaster byr)
        {
            int byrArray;
            bool flag;
            bool flag1 = true;
            byte[] numArray = new byte[4];
            int num = 0;
            int num1 = 0;
            int i = 0;

            ImageFileInfo imageFileInfo = GetImageFileInfo(fStream);
            if (imageFileInfo.type == IMAGE_TYPE.UN_KNOWN)
            {
                throw new ApplicationException("Invalid IFZ file!");
            }

            if (imageFileInfo.type != IMAGE_TYPE.BYR)
            {
                IfzGetCaptureNoMax(fStream, out num);
            }
            if (fStream == null)
            {
                return false;
            }
            try
            {
                try
                {
                    fStream.Read(numArray, 0, 4);
                    int num2 = BitConverter.ToInt32(numArray, 0);
                    if (num2 != -2)
                    {
                        byr.camno = num2;
                        if (byr.camno < 1 || 4 < byr.camno)
                        {
                            flag = false;
                            return flag;
                        }
                        else
                        {
                            byr.ByrArray = new BayerData[byr.camno];
                            for (i = 0; i < byr.camno; i++)
                            {
                                if (imageFileInfo.type == IMAGE_TYPE.BYR)
                                {
                                    byr.ByrArray[i].camerano = i;
                                    byr.ByrArray[i].captureno = 0;
                                }
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].xsize = BitConverter.ToInt32(numArray, 0) - 2;
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].ysize = BitConverter.ToInt32(numArray, 0) - 2;
                                byr.ByrArray[i].x0 = 0;
                                byr.ByrArray[i].y0 = 0;
                                byr.ByrArray[i].width = byr.ByrArray[i].xsize;
                                byr.ByrArray[i].height = byr.ByrArray[i].ysize;
                                byr.ByrArray[i].format = 0;
                                byrArray = (byr.ByrArray[i].xsize + 2) * (byr.ByrArray[i].ysize + 2);
                                byr.ByrArray[i].data = new byte[byrArray];
                                fStream.Read(byr.ByrArray[i].data, 0, byrArray);
                            }
                        }
                    }
                    else
                    {
                        byr.ByrArray = new BayerData[num];
                        for (int j = 0; j < 9999 && fStream.Position < fStream.Length; j++)
                        {
                            if (j > 0)
                            {
                                fStream.Read(numArray, 0, 4);
                            }
                            fStream.Read(numArray, 0, 4);
                            num2 = BitConverter.ToInt32(numArray, 0);
                            byr.camno = 0;
                            for (int k = 0; k < 32; k++)
                            {
                                byr.camno += (num2 & 1);
                                num2 >>= 1;
                            }
                            int num3 = 0;
                            while (num3 < byr.camno)
                            {
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].format = BitConverter.ToInt32(numArray, 0);
                                int num4 = 1;
                                int num5 = 0;
                                if (byr.ByrArray[i].format == 11 && (byr.ByrArray[i].format & 1) == 1)
                                {
                                    num4 = 2;
                                }
                                if (0 <= byr.ByrArray[i].format && byr.ByrArray[i].format <= 3)
                                {
                                    num5 = 2;
                                }
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].xsize = BitConverter.ToInt32(numArray, 0);
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].ysize = BitConverter.ToInt32(numArray, 0);
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].x0 = BitConverter.ToInt32(numArray, 0);
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].y0 = BitConverter.ToInt32(numArray, 0);
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].width = BitConverter.ToInt32(numArray, 0);
                                fStream.Read(numArray, 0, 4);
                                byr.ByrArray[i].height = BitConverter.ToInt32(numArray, 0);
                                fStream.Read(numArray, 0, 4);
                                num2 = BitConverter.ToInt32(numArray, 0);
                                byrArray = (byr.ByrArray[i].width + num5) * (byr.ByrArray[i].height + num5) / num4;
                                if (byr.ByrArray[i].format == 108)
                                {
                                    byrArray *= 2;
                                }
                                else if (byr.ByrArray[i].format == 111)
                                {
                                    byrArray *= 3;
                                }
                                if (num2 - 32 == byrArray)
                                {
                                    byr.ByrArray[i].data = new byte[byrArray];
                                    fStream.Read(byr.ByrArray[i].data, 0, byrArray);
                                    byr.ByrArray[i].camerano = num3;
                                    byr.ByrArray[i].captureno = num1;
                                    if (byrArray > 0)
                                    {
                                        i++;
                                    }
                                    num3++;
                                }
                                else
                                {
                                    flag = false;
                                    return flag;
                                }
                            }
                            num1++;
                        }
                        byr.camno = i;
                    }
                }
                catch
                {
                    flag1 = false;
                }
                return flag1;
            }
            finally
            {
                //fStream.Close();
                fStream.Seek(0, SeekOrigin.Begin);
            }
            return flag;
        }

        private static bool MakeByrData(FileStream fStream, ref BayerMaster byr, IMAGE_TYPE type)
		{
			int byrArray;
			bool flag;
			bool flag1 = true;
			byte[] numArray = new byte[4];
			int num = 0;
			int num1 = 0;
			int i = 0;
			if (type != IMAGE_TYPE.BYR)
			{
				IfzGetCaptureNoMax(fStream, out num);
			}
			if (fStream == null)
			{
				return false;
			}
			try
			{
				try
				{
					fStream.Read(numArray, 0, 4);
					int num2 = BitConverter.ToInt32(numArray, 0);
					if (num2 != -2)
					{
						byr.camno = num2;
						if (byr.camno < 1 || 4 < byr.camno)
						{
							flag = false;
							return flag;
						}
						else
						{
							byr.ByrArray = new BayerData[byr.camno];
							for (i = 0; i < byr.camno; i++)
							{
								if (type == IMAGE_TYPE.BYR)
								{
									byr.ByrArray[i].camerano = i;
									byr.ByrArray[i].captureno = 0;
								}
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].xsize = BitConverter.ToInt32(numArray, 0) - 2;
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].ysize = BitConverter.ToInt32(numArray, 0) - 2;
								byr.ByrArray[i].x0 = 0;
								byr.ByrArray[i].y0 = 0;
								byr.ByrArray[i].width = byr.ByrArray[i].xsize;
								byr.ByrArray[i].height = byr.ByrArray[i].ysize;
								byr.ByrArray[i].format = 0;
								byrArray = (byr.ByrArray[i].xsize + 2) * (byr.ByrArray[i].ysize + 2);
								byr.ByrArray[i].data = new byte[byrArray];
								fStream.Read(byr.ByrArray[i].data, 0, byrArray);
							}
						}
					}
					else
					{
						byr.ByrArray = new BayerData[num];
						for (int j = 0; j < 9999 && fStream.Position < fStream.Length; j++)
						{
							if (j > 0)
							{
								fStream.Read(numArray, 0, 4);
							}
							fStream.Read(numArray, 0, 4);
							num2 = BitConverter.ToInt32(numArray, 0);
							byr.camno = 0;
							for (int k = 0; k < 32; k++)
							{
								byr.camno += (num2 & 1);
								num2 >>= 1;
							}
							int num3 = 0;
							while (num3 < byr.camno)
							{
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].format = BitConverter.ToInt32(numArray, 0);
								int num4 = 1;
								int num5 = 0;
								if (byr.ByrArray[i].format == 11 && (byr.ByrArray[i].format & 1) == 1)
								{
									num4 = 2;
								}
								if (0 <= byr.ByrArray[i].format && byr.ByrArray[i].format <= 3)
								{
									num5 = 2;
								}
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].xsize = BitConverter.ToInt32(numArray, 0);
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].ysize = BitConverter.ToInt32(numArray, 0);
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].x0 = BitConverter.ToInt32(numArray, 0);
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].y0 = BitConverter.ToInt32(numArray, 0);
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].width = BitConverter.ToInt32(numArray, 0);
								fStream.Read(numArray, 0, 4);
								byr.ByrArray[i].height = BitConverter.ToInt32(numArray, 0);
								fStream.Read(numArray, 0, 4);
								num2 = BitConverter.ToInt32(numArray, 0);
								byrArray = (byr.ByrArray[i].width + num5) * (byr.ByrArray[i].height + num5) / num4;
								if (byr.ByrArray[i].format == 108)
								{
									byrArray *= 2;
								}
								else if (byr.ByrArray[i].format == 111)
								{
									byrArray *= 3;
								}
								if (num2 - 32 == byrArray)
								{
									byr.ByrArray[i].data = new byte[byrArray];
									fStream.Read(byr.ByrArray[i].data, 0, byrArray);
									byr.ByrArray[i].camerano = num3;
									byr.ByrArray[i].captureno = num1;
									if (byrArray > 0)
									{
										i++;
									}
									num3++;
								}
								else
								{
									flag = false;
									return flag;
								}
							}
							num1++;
						}
						byr.camno = i;
					}
				}
				catch
				{
					flag1 = false;
				}
				return flag1;
			}
			finally
			{
				fStream.Close();
			}
			return flag;
		}


		private static unsafe bool ScaleChangeForGray(Bitmap oriBmp, out Bitmap destBmp, int ratio)
		{
			int width = oriBmp.Width * ratio / 100;
			int height = oriBmp.Height * ratio / 100;
			destBmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
			Rectangle rectangle = new Rectangle(0, 0, width, height);
			Rectangle rectangle1 = new Rectangle(0, 0, oriBmp.Width, oriBmp.Height);
			BitmapData bitmapDatum = destBmp.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
			BitmapData bitmapDatum1 = oriBmp.LockBits(rectangle1, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			byte* scan0 = (byte*)((void*)bitmapDatum.Scan0);
			byte* numPointer = (byte*)((void*)bitmapDatum1.Scan0);
			int num = Math.Abs(bitmapDatum.Stride);
			int num1 = Math.Abs(bitmapDatum1.Stride);
			double num2 = 100 / (double)ratio;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < num; j++)
				{
					int num3 = (int)(num2 * (double)i) * num1;
					*(scan0 + i * num + j) = *(numPointer + num3 + (int)((double)j * num2));
				}
			}
			destBmp.UnlockBits(bitmapDatum);
			oriBmp.UnlockBits(bitmapDatum1);
			ColorPalette palette = destBmp.Palette;
			for (int k = 0; k < (int)palette.Entries.Length; k++)
			{
				Color color = Color.FromArgb(k, k, k);
				palette.Entries[k] = color;
			}
			destBmp.Palette = palette;
			return true;
		}


		public struct BayerData
		{
			public int xsize;

			public int ysize;

			public int x0;

			public int y0;

			public int width;

			public int height;

			public int format;

			public byte[] data;

			public int camerano;

			public int captureno;
		}

		public struct BayerMaster
		{
			public int camno;

			public BayerData[] ByrArray;
		}
	}
}