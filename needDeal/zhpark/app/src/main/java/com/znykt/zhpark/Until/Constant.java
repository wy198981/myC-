/*
 * Copyright (C) 2010-2013 The SINA WEIBO Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package com.znykt.zhpark.Until;

import com.tencent.mm.sdk.openapi.IWXAPI;

/**
 *  
 * 
 * @author SINA
 * @since 2013-09-29
 */
public class Constant {
	
	
	
	 public static String cloudUrl="http://192.168.2.191:8010";//云服务器地址 用做图片连接
	 
	 public static String apiUrl="http://192.168.2.191:8010/AppApi/";//云服务器接口地址
	
	public static  boolean isLogin=false;// 是否登录标识
	
	
	public static final String QQ_APP_ID = "自己的QQ_APP_ID";
	public static final String WEIXIN_APP_ID = "wxf9a66f885b6a9c59";
	public static final String WEIXIN_APP_SECRET ="8896e876fac743ee4dc6a61dba6090a1";

    public static final String WEIBO_APP_KEY      = "自己的WEIBO_APP_KEY";
    public static final String REDIRECT_URL = "https://api.weibo.com/oauth2/default.html";
    public static final String SCOPE ="email,direct_messages_read,direct_messages_write,"
            + "friendships_groups_read,friendships_groups_write,statuses_to_me_read,"
            + "follow_app_official_microblog," + "invitation_write";
}
