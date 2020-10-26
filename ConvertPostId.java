/*
 * This is the source code of Instagram for Android.
 * It is licensed under GNU GPL v. 2 or later.
 * You should have received a copy of the license in this archive (see LICENSE).
 *
 * Copyright MicroKS, 2015-2020.
 */
package ir.microks.telegram;

import android.text.TextUtils;
import java.math.BigInteger;

public class ConvertPostId {
	
	private static final char[] CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".toCharArray();
	
	/**
	 * convert "https://www.instagram.com/p/AbCdEf/" to PostId
	 *
	 * @param shortCode shortCode AbCdEf
	 * @param userId 
	 * @return post id "123456789_123456"
	 */
	public static String toPostId(String shortCode, long userId) {
		String mediaId = "";
		for (int i = 0; i < shortCode.length(); i++) {
			int offset = 0;
			for (int i2 = 0; i2 < CHARACTERS.length; i2++) {
				if (CHARACTERS[i2] == shortCode.charAt(i)) {
					offset = i2;
					break;
				}
			}
			
			String binary = Integer.toBinaryString(offset);
			mediaId += "000000".substring(binary.length()) + binary;
		}
		return new BigInteger(mediaId, 2).longValue() + "_" + userId;
	}

	/**
	 * convert "123456789_123456" to ShortCode
	 *
	 * @param postId Example: MediaId_UserId "123456789_123456" or only MediaId "123456789"
	 * @return post URL
	 */
    public static String totShortCode(String postId) {
        long mediaId = 0;
		if (postId.contains("_")) {
			String[] ids = postId.split("_");
			mediaId =  parseLong(ids[0]);
			//userId = parseLong(ids[1]);
		} else {
			mediaId = parseLong(postId);
		}

		String shortCode = "";
		while (mediaId > 0) {
			long remainder = mediaId % 64;
			mediaId = (mediaId - remainder) / 64;
			shortCode = CHARACTERS[(int) remainder] + shortCode;
		}
		//return "https://www.instagram.com/tv/" + shortCode + "/";//TODO for IG TV
		return "https://www.instagram.com/p/" + shortCode + "/";
	}

    private static long parseLong(String number) {
        if (TextUtils.isEmpty(number)) {
            return 0L;
        }
        try {
            number = number.replaceAll("[^\\d\\-]+","").replaceAll("(?<!^)\\-", "");
            return number.replace("-", "").equals("") ? 0L : Long.parseLong(number);
        } catch (NumberFormatException e) {
            return 0L;
        }
    }
	
}
