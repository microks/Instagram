/*
 * This is the source code of Instagram for php.
 * It is licensed under GNU GPL v. 2 or later.
 * You should have received a copy of the license in this archive (see LICENSE).
 *
 * Copyright MicroKS, 2015-2020.
 */
<?php

/**
 * convert "https://www.instagram.com/p/AbCdEf/" to PostId
 * 
 * @param string $shortCode AbCdEf
 * @param int $userId
 * @return string post id "123456789_123456"
 */
function toPostId($shortCode, $userId) {
	$CHARACTERS = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_';
	$mediaId = "";
	for ($i = 0; $i < strlen($shortCode); $i++) {
		$offset = 0;
		for ($i2 = 0; $i2 < strlen($CHARACTERS); $i2++) {
			if ($CHARACTERS{$i2} == $shortCode{$i}) {
				$offset = $i2;
				break;
			}
		}
		$mediaId .= str_pad(decbin($offset), 6, '0', STR_PAD_LEFT);
	}
	return bindec($mediaId) . "_" . $userId;
}

/**
 * convert "123456789_123456" to ShortCode
 * 
 * @param string $postId Example: MediaId_UserId "123456789_123456" or only MediaId "123456789"
 * @return string post URL
 */
function totShortCode($postId) {
	if (strpos($postId, '_') !== false) {
		$postId = explode('_', $postId);
		$mediaId = (int) $postId[0];
		//$userId = (int) $postId[1];
	} else {
		$mediaId = (int) $postId;
	}
	
	$CHARACTERS = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_';
	$shortCode = "";
	while ($mediaId > 0) {
		$remainder = $mediaId % 64;
		$mediaId = ($mediaId - $remainder) / 64;
		$shortCode = $CHARACTERS{$remainder} . $shortCode;
	}
	//return "https://www.instagram.com/tv/" . $shortCode . "/";//TODO for IG TV
	return "https://www.instagram.com/p/" . $shortCode . "/";
}

?>
