/*
 * IconParader.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Linq;
using Cadencii.Gui;

namespace cadencii
{
	public class IconParaderControllerWF : IconParaderController
	{
		public override IconImageCreator DoCreateIconImage {
			get {
				return (string path_image, string singer_name) => {
#if DEBUG
					Logger.StdOut ("IconParader#createIconImage; path_image=" + path_image);
#endif
					Image ret = null;
					if (System.IO.File.Exists (path_image)) {
						System.IO.FileStream fs = null;
						try {
							fs = new System.IO.FileStream (path_image, System.IO.FileMode.Open, System.IO.FileAccess.Read);
							System.Drawing.Image img = System.Drawing.Image.FromStream (fs);
							ret.NativeImage = img;
						} catch (Exception ex) {
							Logger.StdErr ("IconParader#createIconImage; ex=" + ex);
						} finally {
							if (fs != null) {
								try {
									fs.Close ();
								} catch (Exception ex2) {
									Logger.StdErr ("IconParader#createIconImage; ex2=" + ex2);
								}
							}
						}
					}

					if (ret == null) {
						// 画像ファイルが無かったか，読み込みに失敗した場合

						// 歌手名が描かれた画像をセットする
						Image bmp = new Image (ICON_WIDTH, ICON_HEIGHT);
						Graphics g = new Graphics (bmp);
						g.clearRect (0, 0, ICON_WIDTH, ICON_HEIGHT);
						Font font = new Font (System.Windows.Forms.SystemInformation.MenuFont);
						g.drawStringEx (
							singer_name, font, new Rectangle (1, 1, ICON_WIDTH - 2, ICON_HEIGHT - 2),
							Graphics.STRING_ALIGN_NEAR, Graphics.STRING_ALIGN_NEAR);
						ret = bmp;
					}

					return ret;
				};
			}
		}
	}

}
