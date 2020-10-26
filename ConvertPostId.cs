/*
 * This is the source code of Instagram for C#.
 * It is licensed under GNU GPL v. 2 or later.
 * You should have received a copy of the license in this archive (see LICENSE).
 *
 * Copyright MicroKS, 2015-2020.
 */
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;

namespace Instagram {
	public class ConverPostId {

		private static readonly char[] CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".ToCharArray();

		/// <summary>
		/// convert "https://www.instagram.com/p/AbCdEf/" to PostId
		/// </summary>
		/// <param name="shortCode">AbCdEf</param>
		/// <param name="userId"></param>
		/// <returns>post id "123456789_123456"</returns>
		///
		public static string ToPostId(string shortCode, long userId) {
			string mediaId = "";
			for (int i = 0; i < shortCode.Length; i++) {
				int offset = 0;
				for (int i2 = 0; i2 < CHARACTERS.Length; i2++) {
					if (CHARACTERS[i2] == shortCode[i]) {
						offset = i2;
						break;
					}
				}
				mediaId += Convert.ToString(offset, 2).PadLeft(6, '0');
			}
			return Convert.ToInt64(mediaId, 2) + "_" + userId;
		}

		/// <summary>
		/// convert "123456789_123456" to ShortCode
		/// </summary>
		/// <param name="postId">MediaId_UserId "123456789_123456" or only MediaId "123456789"</param>
		/// <returns>post URL</returns>
		///
		public static string TotShortCode(string postId) {
			long mediaId = 0;
			if (postId.Contains("_")) {
				string[] ids = postId.Split('_');
				mediaId = StringUtils.ParseLong(ids[0]);
				//userId = StringUtils.ParseLong(ids[1]);
			} else {
				mediaId = ParseLong(postId);
			}
			
			string shortCode = "";
			while (mediaId > 0) {
				long remainder = mediaId % 64;
				mediaId = (mediaId - remainder) / 64;
				shortCode = CHARACTERS[(int) remainder] + shortCode;
			}
			//return "https://www.instagram.com/tv/" + shortCode + "/";//-- for IG TV
			return "https://www.instagram.com/p/" + shortCode + "/";
		}

		private static long ParseLong(string str) {
			if (String.IsNullOrEmpty(str)) {
				return 0L;
			}
			str = Regex.Replace(str, "[^\\d\\-]+", "");
			str = Regex.Replace(str, "(?<!^)\\-", "");
			try {
				return str == "" ? 0L : long.Parse(str, CultureInfo.InvariantCulture.NumberFormat);
			} catch (Exception) { }
			return 0L;
		}
	}
}
