import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class FormBezierPointEdit extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private JButton btnBackward = null;
	private JCheckBox chkEnableSmooth = null;
	private JButton btnForward = null;
	private BGroupBox groupLeft = null;
	private JLabel lblLeftClock = null;
	private JTextField jTextField = null;
	private JLabel lblLeftValue = null;
	private JTextField jTextField1 = null;
	private JButton btnLeft = null;
	private BGroupBox groupDataPoint = null;
	private JLabel lblDataPointClock = null;
	private JTextField jTextField2 = null;
	private JLabel lblDataPointValue = null;
	private JTextField jTextField11 = null;
	private JButton btnDataPoint = null;
	private BGroupBox groupRight = null;
	private JLabel lblRightClock = null;
	private JTextField jTextField3 = null;
	private JLabel lblRightValue = null;
	private JTextField jTextField12 = null;
	private JButton btnRight = null;
	private JButton btnOk = null;
	private JButton btnCancel = null;
	private JLabel jLabel4 = null;
	private JLabel jLabel5 = null;
	private JPanel jPanel3 = null;
	/**
	 * This is the default constructor
	 */
	public FormBezierPointEdit() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(469, 266);
		this.setContentPane(getJContentPane());
		this.setTitle("Edit Bezier Data Point");
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane() {
		if (jContentPane == null) {
			GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
			gridBagConstraints91.gridx = 0;
			gridBagConstraints91.gridwidth = 3;
			gridBagConstraints91.anchor = GridBagConstraints.EAST;
			gridBagConstraints91.gridy = 4;
			GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			gridBagConstraints81.gridx = 0;
			gridBagConstraints81.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints81.gridwidth = 3;
			gridBagConstraints81.gridy = 3;
			jLabel5 = new JLabel();
			jLabel5.setText("    ");
			GridBagConstraints gridBagConstraints73 = new GridBagConstraints();
			gridBagConstraints73.gridx = 0;
			gridBagConstraints73.gridwidth = 3;
			gridBagConstraints73.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints73.gridy = 1;
			jLabel4 = new JLabel();
			jLabel4.setText("     ");
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 2;
			gridBagConstraints13.gridy = 1;
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.gridx = 2;
			gridBagConstraints10.weightx = 1.0D;
			gridBagConstraints10.fill = GridBagConstraints.BOTH;
			gridBagConstraints10.insets = new Insets(5, 5, 5, 5);
			gridBagConstraints10.gridy = 2;
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.gridx = 1;
			gridBagConstraints9.weightx = 1.0D;
			gridBagConstraints9.fill = GridBagConstraints.BOTH;
			gridBagConstraints9.insets = new Insets(5, 5, 5, 5);
			gridBagConstraints9.gridy = 2;
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.gridx = 0;
			gridBagConstraints8.weightx = 1.0D;
			gridBagConstraints8.fill = GridBagConstraints.BOTH;
			gridBagConstraints8.insets = new Insets(5, 5, 5, 5);
			gridBagConstraints8.gridy = 2;
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.anchor = GridBagConstraints.WEST;
			gridBagConstraints2.gridy = 0;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.gridx = 1;
			gridBagConstraints1.gridy = 0;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.EAST;
			gridBagConstraints.gridy = 0;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(getBtnBackward(), gridBagConstraints);
			jContentPane.add(getChkEnableSmooth(), gridBagConstraints1);
			jContentPane.add(getBtnForward(), gridBagConstraints2);
			jContentPane.add(getGroupLeft(), gridBagConstraints8);
			jContentPane.add(getGroupDataPoint(), gridBagConstraints9);
			jContentPane.add(getGroupRight(), gridBagConstraints10);
			jContentPane.add(jLabel4, gridBagConstraints73);
			jContentPane.add(jLabel5, gridBagConstraints81);
			jContentPane.add(getJPanel3(), gridBagConstraints91);
			jContentPane.add(jLabel4, gridBagConstraints13);
		}
		return jContentPane;
	}

	/**
	 * This method initializes btnBackward	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnBackward() {
		if (btnBackward == null) {
			btnBackward = new JButton();
			btnBackward.setText("<<");
		}
		return btnBackward;
	}

	/**
	 * This method initializes chkEnableSmooth	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkEnableSmooth() {
		if (chkEnableSmooth == null) {
			chkEnableSmooth = new JCheckBox();
			chkEnableSmooth.setText("Smooth");
		}
		return chkEnableSmooth;
	}

	/**
	 * This method initializes btnForward	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnForward() {
		if (btnForward == null) {
			btnForward = new JButton();
			btnForward.setText(">>");
		}
		return btnForward;
	}

	/**
	 * This method initializes groupLeft	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupLeft() {
		if (groupLeft == null) {
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 0;
			gridBagConstraints7.gridwidth = 2;
			gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints7.weightx = 1.0D;
			gridBagConstraints7.ipadx = 0;
			gridBagConstraints7.ipady = 0;
			gridBagConstraints7.insets = new Insets(5, 20, 5, 20);
			gridBagConstraints7.gridy = 3;
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints6.gridy = 1;
			gridBagConstraints6.weightx = 1.0;
			gridBagConstraints6.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints6.gridx = 1;
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 0;
			gridBagConstraints5.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints5.gridy = 1;
			lblLeftValue = new JLabel();
			lblLeftValue.setText("Value");
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints4.gridy = 0;
			gridBagConstraints4.weightx = 1.0D;
			gridBagConstraints4.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints4.gridx = 1;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints3.gridy = 0;
			lblLeftClock = new JLabel();
			lblLeftClock.setText("Clock");
			groupLeft = new BGroupBox();
			groupLeft.setLayout(new GridBagLayout());
			groupLeft.setTitle("Left Control Point");
			groupLeft.add(lblLeftClock, gridBagConstraints3);
			groupLeft.add(getJTextField(), gridBagConstraints4);
			groupLeft.add(lblLeftValue, gridBagConstraints5);
			groupLeft.add(getJTextField1(), gridBagConstraints6);
			groupLeft.add(getBtnLeft(), gridBagConstraints7);
		}
		return groupLeft;
	}

	/**
	 * This method initializes jTextField	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getJTextField() {
		if (jTextField == null) {
			jTextField = new JTextField();
		}
		return jTextField;
	}

	/**
	 * This method initializes jTextField1	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getJTextField1() {
		if (jTextField1 == null) {
			jTextField1 = new JTextField();
		}
		return jTextField1;
	}

	/**
	 * This method initializes btnLeft	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnLeft() {
		if (btnLeft == null) {
			btnLeft = new JButton();
			btnLeft.setText("");
			btnLeft.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		}
		return btnLeft;
	}

	/**
	 * This method initializes groupDataPoint	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupDataPoint() {
		if (groupDataPoint == null) {
			GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			gridBagConstraints71.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints71.gridx = 0;
			gridBagConstraints71.gridy = 2;
			gridBagConstraints71.weightx = 1.0D;
			gridBagConstraints71.insets = new Insets(5, 20, 5, 20);
			gridBagConstraints71.gridwidth = 2;
			GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			gridBagConstraints61.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints61.gridy = 1;
			gridBagConstraints61.weightx = 1.0;
			gridBagConstraints61.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints61.gridx = 1;
			GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			gridBagConstraints51.gridx = 0;
			gridBagConstraints51.anchor = GridBagConstraints.WEST;
			gridBagConstraints51.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints51.gridy = 1;
			lblDataPointValue = new JLabel();
			lblDataPointValue.setText("Value");
			GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
			gridBagConstraints41.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints41.gridy = 0;
			gridBagConstraints41.weightx = 1.0D;
			gridBagConstraints41.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints41.gridx = 1;
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.gridx = 0;
			gridBagConstraints31.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints31.anchor = GridBagConstraints.WEST;
			gridBagConstraints31.gridy = 0;
			lblDataPointClock = new JLabel();
			lblDataPointClock.setText("Clock");
			groupDataPoint = new BGroupBox();
			groupDataPoint.setLayout(new GridBagLayout());
			groupDataPoint.setTitle("Data Point");
			groupDataPoint.add(lblDataPointClock, gridBagConstraints31);
			groupDataPoint.add(getJTextField2(), gridBagConstraints41);
			groupDataPoint.add(lblDataPointValue, gridBagConstraints51);
			groupDataPoint.add(getJTextField11(), gridBagConstraints61);
			groupDataPoint.add(getBtnDataPoint(), gridBagConstraints71);
		}
		return groupDataPoint;
	}

	/**
	 * This method initializes jTextField2	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getJTextField2() {
		if (jTextField2 == null) {
			jTextField2 = new JTextField();
		}
		return jTextField2;
	}

	/**
	 * This method initializes jTextField11	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getJTextField11() {
		if (jTextField11 == null) {
			jTextField11 = new JTextField();
		}
		return jTextField11;
	}

	/**
	 * This method initializes btnDataPoint	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnDataPoint() {
		if (btnDataPoint == null) {
			btnDataPoint = new JButton();
			btnDataPoint.setText("");
			btnDataPoint.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		}
		return btnDataPoint;
	}

	/**
	 * This method initializes groupRight	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupRight() {
		if (groupRight == null) {
			GridBagConstraints gridBagConstraints72 = new GridBagConstraints();
			gridBagConstraints72.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints72.gridx = 0;
			gridBagConstraints72.gridy = 2;
			gridBagConstraints72.weightx = 1.0D;
			gridBagConstraints72.insets = new Insets(5, 20, 5, 20);
			gridBagConstraints72.gridwidth = 2;
			GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
			gridBagConstraints62.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints62.gridy = 1;
			gridBagConstraints62.weightx = 1.0;
			gridBagConstraints62.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints62.gridx = 1;
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.gridx = 0;
			gridBagConstraints52.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints52.gridy = 1;
			lblRightValue = new JLabel();
			lblRightValue.setText("Value");
			GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			gridBagConstraints42.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints42.gridy = 0;
			gridBagConstraints42.weightx = 1.0D;
			gridBagConstraints42.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints42.gridx = 1;
			GridBagConstraints gridBagConstraints32 = new GridBagConstraints();
			gridBagConstraints32.gridx = 0;
			gridBagConstraints32.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints32.gridy = 0;
			lblRightClock = new JLabel();
			lblRightClock.setText("Clock");
			groupRight = new BGroupBox();
			groupRight.setLayout(new GridBagLayout());
			groupRight.setTitle("Right Control Point");
			groupRight.add(lblRightClock, gridBagConstraints32);
			groupRight.add(getJTextField3(), gridBagConstraints42);
			groupRight.add(lblRightValue, gridBagConstraints52);
			groupRight.add(getJTextField12(), gridBagConstraints62);
			groupRight.add(getBtnRight(), gridBagConstraints72);
		}
		return groupRight;
	}

	/**
	 * This method initializes jTextField3	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getJTextField3() {
		if (jTextField3 == null) {
			jTextField3 = new JTextField();
		}
		return jTextField3;
	}

	/**
	 * This method initializes jTextField12	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getJTextField12() {
		if (jTextField12 == null) {
			jTextField12 = new JTextField();
		}
		return jTextField12;
	}

	/**
	 * This method initializes btnRight	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnRight() {
		if (btnRight == null) {
			btnRight = new JButton();
			btnRight.setText("");
			btnRight.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		}
		return btnRight;
	}

	/**
	 * This method initializes btnOk	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnOk() {
		if (btnOk == null) {
			btnOk = new JButton();
			btnOk.setText("OK");
		}
		return btnOk;
	}

	/**
	 * This method initializes btnCancel	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnCancel() {
		if (btnCancel == null) {
			btnCancel = new JButton();
			btnCancel.setText("Cancel");
		}
		return btnCancel;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel3() {
		if (jPanel3 == null) {
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 0;
			gridBagConstraints11.insets = new Insets(0, 0, 0, 16);
			gridBagConstraints11.gridy = 0;
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.gridx = 2;
			gridBagConstraints12.insets = new Insets(0, 0, 0, 16);
			gridBagConstraints12.gridy = 0;
			jPanel3 = new JPanel();
			jPanel3.setLayout(new GridBagLayout());
			jPanel3.add(getBtnCancel(), gridBagConstraints12);
			jPanel3.add(getBtnOk(), gridBagConstraints11);
		}
		return jPanel3;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
