package com.znykt.zhpark.Until;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.cert.X509Certificate;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;
/**
 * @description 鏈変竴涓俊浠荤鐞嗗櫒绫昏礋璐ｅ喅瀹氭槸鍚︿俊浠昏繙绔殑璇佷�?  X509TrustManager锛屾洿澶氱Щ鍔ㄥ紑鍙戝唴�?�硅鍏虫敞锛� http://blog.csdn.net/xiong_it
 * @charset UTF-8
 * @author xiong_it
 * @date 2015-7-16涓嬪�?5:38:38
 * @version 
 */
public class HTTPSTrustManager implements X509TrustManager {  
	  
    private static TrustManager[] trustManagers;  
    private static final X509Certificate[] _AcceptedIssuers = new X509Certificate[] {};  
  
    /**
     * 璇ユ柟娉曟鏌ュ鎴风鐨勮瘉涔︼紝鑻ヤ笉淇′换璇ヨ瘉涔�?垯鎶涘嚭寮傚父銆傜敱浜庢垜浠笉闇�瑕佸�?�㈡埛绔繘琛岃璇侊紝
     * 鍥犳鎴戜滑鍙渶瑕佹墽琛岄粯璁ょ殑淇�?�换绠＄悊鍣ㄧ殑杩欎釜鏂规硶銆侸SSE涓紝榛樿鐨勪俊浠荤鐞嗗櫒绫讳负TrustManager銆�
     */
    @Override  
    public void checkClientTrusted(  
            X509Certificate[] x509Certificates, String s)
            throws java.security.cert.CertificateException {  
        // To change body of implemented methods use File | Settings | File  
        // Templates.  
    }  
    
  /**
   * 璇ユ柟娉曟鏌ユ湇鍔″櫒鐨勮瘉涔︼紝鑻ヤ笉淇′换璇ヨ瘉涔�?悓鏍锋姏鍑哄紓甯搞�傞�氳繃鑷繁�?�炵幇璇ユ柟娉曪紝鍙互浣夸箣淇′换鎴戜滑鎸囧畾鐨勪换浣曡瘉涔︺�傚湪�?�炵幇璇ユ柟娉曟椂锛�?
   * 涔熷彲浠ョ畝鍗曠殑涓嶅仛浠讳綍澶勭悊锛屽嵆涓�涓┖鐨勫嚱鏁颁綋锛岀敱浜庝笉浼氭姏鍑哄紓甯革紝�?�冨氨浼氫俊浠讳换浣曡瘉涔︺��
   */
    @Override  
    public void checkServerTrusted(  
            X509Certificate[] x509Certificates, String s)
            throws java.security.cert.CertificateException {  
        // To change body of implemented methods use File | Settings | File  
        // Templates.  
    }  
  
    public boolean isClientTrusted(X509Certificate[] chain) {  
        return true;  
    }  
  
    public boolean isServerTrusted(X509Certificate[] chain) {  
        return true;  
    }  
  
    @Override  
    public X509Certificate[] getAcceptedIssuers() {  
        return _AcceptedIssuers;  
    }  
  
    public static void allowAllSSL() {  
        HttpsURLConnection.setDefaultHostnameVerifier(new HostnameVerifier() {  
  
            @Override  
            public boolean verify(String arg0, SSLSession arg1) {  
                return true;  
            }  
  
        });  
  
        SSLContext context = null;  
        if (trustManagers == null) {  
            trustManagers = new TrustManager[] { new HTTPSTrustManager() };  
        }  
  
        try {  
            context = SSLContext.getInstance("TLS");  
            context.init(null, trustManagers, new SecureRandom());  
        } catch (NoSuchAlgorithmException e) {  
            e.printStackTrace();  
        } catch (KeyManagementException e) {  
            e.printStackTrace();  
        }  
  
        HttpsURLConnection.setDefaultSSLSocketFactory(context  
                .getSocketFactory());  
    }  
  
} 