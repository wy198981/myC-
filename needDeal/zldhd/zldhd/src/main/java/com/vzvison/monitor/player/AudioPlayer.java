package com.vzvison.monitor.player;

import java.util.ArrayList;
import java.util.List;

import android.content.Context;
import android.media.AudioFormat;
import android.media.AudioManager;
import android.media.AudioTrack;
import android.media.MediaPlayer;

/**
 * 闊抽鎾斁鍣ㄧ被
 * @author 璋眽锟??
 * @date 2012-8-3
 */
public class AudioPlayer {
	private int sampleRateInHz = 8000; //閲囨牱閫熺巼
	private int channelConfig = AudioFormat.CHANNEL_OUT_MONO; //鍗曞０锟??
	private int audioEncoding = AudioFormat.ENCODING_PCM_16BIT; //閲囨牱娣卞害
	private int streamType = AudioManager.STREAM_MUSIC;
	private int mode = AudioTrack.MODE_STREAM;
	
	private AudioTrack mAudioTrack = null;
	private MediaPlayer mediaPlayer = null;
	private boolean isAudioPlaying = false;
	private boolean isAlarmPlaying = false;
	private Context context = null;
	
	private List<byte[]> audioDataLists = new ArrayList<byte[]>();
	private float volume = 0.7f;
	
	public boolean isAudioPlaying() {
		return isAudioPlaying;
	}
	
	public boolean isAlarmPlaying() {
		return isAlarmPlaying;
	}
	
	public AudioPlayer(Context context) {
		this.context = context;
	}
	
	public AudioPlayer(Context context,int channel, int encoding,int sampleRateInHz) {
		this.context = context;
		this.channelConfig = channel;
		this.audioEncoding = encoding;
		this.sampleRateInHz = sampleRateInHz;
	}
	
	/**
	 * 璁剧疆闊抽鏁版嵁
	 * @param data 闊抽鏁版嵁
	 */
	public synchronized void addAudioData(byte[] audioData) {
//		if(null != audioData) {
//			audioDataLists.add(audioData);
//		}
		mAudioTrack.write(audioData, 0, audioData.length);
	}
	
	/**
	 * 鑾峰彇闊抽鏁版嵁鍒楄〃鐨勫ご浣嶇疆鏁版嵁
	 * @return 濡傛灉鏈夐煶棰戞暟鎹紝灏辫繑鍥為煶棰戞暟鎹紝鍚﹀垯杩斿洖闀垮害锟??鐨勬暟锟??
	 */
	private synchronized byte[] getAudioData() {
		if(audioDataLists.size() > 0) {
			return audioDataLists.remove(0);
		} else {
			return new byte[0];
		}
	}
	
	/**
	 * 娓呯┖闊抽鏁版嵁
	 */
	private void clearAudioData() {
		audioDataLists.clear();
	}
	
	/**
	 * 鍒濆锟??
	 */
	private void initAudioTrack() {
		int bufferSizeInBytes = AudioTrack.getMinBufferSize(sampleRateInHz, channelConfig, audioEncoding);
	    mAudioTrack = new AudioTrack(streamType, sampleRateInHz, channelConfig, audioEncoding, bufferSizeInBytes, mode);
	    mAudioTrack.setStereoVolume(volume, volume);
	}
	
	/**
	 * 鎾斁澹伴煶
	 */
	public void play() {
		initAudioTrack();
	    isAudioPlaying = true;
	    mAudioTrack.play();
//	    AudioPlayThread audioPlayThread = new AudioPlayThread();
//	    audioPlayThread.start();
	}
	
	/**
	 * 鍋滄澹伴煶
	 */
	public void stop() {
		isAudioPlaying = false;
		if(null != mAudioTrack) {
			mAudioTrack.stop();
			mAudioTrack.release();
			mAudioTrack = null;
		}
		clearAudioData();
	}
	
//	/**
//	 * 鎾斁璀︽姤澹伴煶
//	 */
//	public void playAlarm() {
//		mediaPlayer = MediaPlayer.create(context, com.huanyi.monitor.R.raw.alarm);
//		if(null != mediaPlayer) {
//		    mediaPlayer.start();
//		}
//		isAlarmPlaying = true;
//	}
//	
//	/**
//	 * 鍋滄璀︽姤澹伴煶
//	 */
//	public void stopAlarm() {
//		isAlarmPlaying = false;
//		if (null != mediaPlayer) {
//			mediaPlayer.stop();
//			mediaPlayer.release();
//			mediaPlayer = null;
//		} 
//	}
	
	/**
	 * 闊抽鎾斁鐨勭嚎绋嬬被
	 * @author Administrator
	 */
	private class AudioPlayThread extends Thread {
		@Override
		public void run() {
			playing(); 
		}

		private void playing() {
			while(isAudioPlaying) {
				byte[] data = getAudioData();
				if(null != data && data.length > 0) {
					mAudioTrack.write(data, 0, data.length);
				} else {
					try {
						Thread.sleep(5);
					} catch (InterruptedException e) {
					}
				}
				
			}
		}
	}
}
