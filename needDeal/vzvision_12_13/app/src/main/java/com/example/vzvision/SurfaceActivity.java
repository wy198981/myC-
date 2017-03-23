package com.example.vzvision;

import android.app.Activity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;

import java.nio.IntBuffer;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.opengl.GLSurfaceView;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.widget.*;
import android.view.*;

public class SurfaceActivity extends Activity {
	
	 private GLSurfaceView mEffectView;

	    private TextureRenderer renderer;
	    private  RelativeLayout layout;
	    

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_surface);
		
		
		 renderer = new TextureRenderer();
		 
		    String mstrTitle = "Œﬁ ”∆µ";
		    Bitmap bmp = Bitmap.createBitmap(256, 256, Bitmap.Config.ARGB_8888);
	        Canvas canvasTemp = new Canvas(bmp);
	        canvasTemp.drawColor(Color.BLACK);
	        Paint p = new Paint();
	        String familyName = "ÀŒÃÂ";
	        Typeface font = Typeface.create(familyName, Typeface.BOLD);
	        p.setColor(Color.RED);
	        p.setTypeface(font);
	        p.setTextSize(27);
	        canvasTemp.drawText(mstrTitle, 0, 100, p);
		 
	       // renderer.setImageBitmap(BitmapFactory.decodeResource(getResources(), R.drawable.ic_1));
	        renderer.setImageBitmap(bmp);
	        renderer.setCurrentEffect(R.id.none);
	        
	        
	        layout = (RelativeLayout)findViewById(R.id.relativeLayout_surface);
	        
	        try{
	        	 mEffectView = new GLSurfaceView(this);
	        }
	        catch  (Exception e)
	        {
	        	int a;
	        	a = 0;
	        }
	        
	       
	         ViewGroup.LayoutParams lp = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FILL_PARENT,ViewGroup.LayoutParams.FILL_PARENT);
	         mEffectView.setLayoutParams(lp );
	         
	          
	        mEffectView.setEGLContextClientVersion(2);
	        //mEffectView.setRenderer(this);
	        mEffectView.setRenderer(renderer);
	       mEffectView.setRenderMode(GLSurfaceView.RENDERMODE_WHEN_DIRTY);
	        
	        
	        layout.addView(mEffectView);
	        
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		
		  Log.i("info", "menu create");
	        MenuInflater inflater = getMenuInflater();
	        inflater.inflate(R.menu.surface, menu);
		
		//getMenuInflater().inflate(R.menu.surface, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
//		int id = item.getItemId();
//		if (id == R.id.action_settings) {
//			return true;
//		}
//		return super.onOptionsItemSelected(item);
		
		renderer.setCurrentEffect(item.getItemId());
        mEffectView.requestRender();
        
        return true;
	}
}
