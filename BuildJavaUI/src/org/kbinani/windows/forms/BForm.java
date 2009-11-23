package org.kbinani.windows.forms;

import java.awt.Dimension;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import javax.swing.JFrame;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BForm extends JFrame implements WindowListener, KeyListener{
    private static final long serialVersionUID = -3700177079249925623L;
    public BEvent<BFormClosingEventHandler> formClosingEvent = new BEvent<BFormClosingEventHandler>();
    public BEvent<BEventHandler> formClosedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> activatedEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> deactivateEvent = new BEvent<BEventHandler>();
    public BEvent<BEventHandler> loadEvent = new BEvent<BEventHandler>();
    private BDialogResult m_result = BDialogResult.CANCEL;
    private boolean m_closed = false;
    
    public BForm(){
        this( "" );
    }
    
    public BForm( String title ){
        super( title );
        addWindowListener( this );
        addKeyListener( this );
    }
    
    public Dimension getClientSize(){
        return getContentPane().getSize();
    }
    
    public void close(){
        setVisible( false );
        try{
            BFormClosingEventArgs e = new BFormClosingEventArgs();
            formClosingEvent.raise( this, e );
            if( e.Cancel ){
                setVisible( true );
                return;
            }
        }catch( Exception ex ){
            System.err.println( "BForm#close; ex=" + ex );
        }
        dispose();
    }
    
    public void windowActivated( WindowEvent e ){
        try{
            activatedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowActivated; ex=" + ex );
        }
    }
    
    public void windowClosed( WindowEvent e ){
        m_closed = true;
        try{
            formClosedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowClosed; ex=" + ex );
        }
    }
    
    public void windowClosing( WindowEvent e ){
        try{
            formClosingEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowClosing; ex=" + ex );
        }
    }
    
    public void windowDeactivated( WindowEvent e ){
        try{
            deactivateEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowDeactivated; ex=" + ex );
        }
    }
    
    public void windowDeiconified( WindowEvent e ){
    }
    
    public void windowIconified( WindowEvent e ){
    }
    
    public void windowOpened( WindowEvent e ){
        try{
            loadEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.out.println( "BForm#windowOpened; ex=" + ex );
        }
    }
    
    public class ShowDialogRunner implements Runnable{
        public void run(){
            setVisible( true );
            while( !m_closed ){
                try{
                    Thread.sleep( 100 );
                }catch( Exception ex ){
                    break;
                }
            }
            setVisible( false );
        }
    }
    
    public BDialogResult showDialog(){
        try{
            Thread t = new Thread( new ShowDialogRunner() );
            t.start();
            t.join();
        }catch( Exception ex ){
            System.out.println( "BForm#showDialog; ex=" + ex );
        }
        return m_result;
    }
    
    public BDialogResult getDialogResult(){
        return m_result;
    }
    
    public void setDialogResult( BDialogResult value ){
        m_closed = true;
        m_result = value;
    }
    
    /* root implementation of java.awt.Component */
    /* REGION java.awt.Component */
    /* root implementation of java.awt.Component instanceof in BForm.cs(java) */
    public BEvent<BKeyEventHandler> keyUpEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyDownEvent = new BEvent<BKeyEventHandler>();
    public BEvent<BKeyEventHandler> keyPressedEvent = new BEvent<BKeyEventHandler>();
    
    public void keyPressed( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyDownEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyPressed; ex=" + ex );
        }
    }
    
    public void keyReleased( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyUpEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyReleased; ex=" + ex );
        }
    }
    
    public void keyTyped( KeyEvent e0 ){
        try{
            BKeyEventArgs e = new BKeyEventArgs( e0 );
            keyPressedEvent.raise( this, e );
        }catch( Exception ex ){
            System.err.println( "BForm#keyTyped; ex=" + ex );
        }
    }
    /* END REGION java.awt.Component */
}