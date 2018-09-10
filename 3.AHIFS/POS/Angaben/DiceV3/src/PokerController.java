
import java.util.ArrayList;
import javax.swing.DefaultListModel;

/**
 *
 * @author Gerald
 */
public class PokerController extends javax.swing.JFrame {
    /*
    pattern: Singleton
    */
    public static PokerController newInstance() {
        if (pc == null) {
            pc = new PokerController();
        }
        return pc;
    }
    /**
     * Creates new form PokerController
     */
    private PokerController() {
        initComponents();
        initOtherThings();
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jLabel1 = new javax.swing.JLabel();
        txtMessage = new javax.swing.JLabel();
        jLabel3 = new javax.swing.JLabel();
        txtNumberOfPlayers = new javax.swing.JTextField();
        btnGenerate = new javax.swing.JButton();
        panelplayerInfo = new javax.swing.JScrollPane();
        listPlayerInfo = new javax.swing.JList<>();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        getContentPane().setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        jLabel1.setBackground(new java.awt.Color(204, 255, 204));
        jLabel1.setFont(new java.awt.Font("Woodcut", 3, 18)); // NOI18N
        jLabel1.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        jLabel1.setText("Poker Controller (V2)");
        jLabel1.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.RAISED, new java.awt.Color(51, 51, 255), new java.awt.Color(0, 0, 102)));
        jLabel1.setOpaque(true);
        getContentPane().add(jLabel1, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 20, 570, 50));

        txtMessage.setBackground(new java.awt.Color(255, 204, 0));
        txtMessage.setText("enter number of players and press GENERATE");
        txtMessage.setOpaque(true);
        getContentPane().add(txtMessage, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 280, 620, -1));

        jLabel3.setText("# of players:");
        getContentPane().add(jLabel3, new org.netbeans.lib.awtextra.AbsoluteConstraints(20, 100, -1, -1));

        txtNumberOfPlayers.setHorizontalAlignment(javax.swing.JTextField.RIGHT);
        txtNumberOfPlayers.setText("2");
        txtNumberOfPlayers.setToolTipText("");
        getContentPane().add(txtNumberOfPlayers, new org.netbeans.lib.awtextra.AbsoluteConstraints(100, 100, 30, -1));

        btnGenerate.setText("Generate");
        btnGenerate.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                PokerController.this.actionPerformed(evt);
            }
        });
        getContentPane().add(btnGenerate, new org.netbeans.lib.awtextra.AbsoluteConstraints(170, 100, -1, -1));

        panelplayerInfo.setViewportView(listPlayerInfo);

        getContentPane().add(panelplayerInfo, new org.netbeans.lib.awtextra.AbsoluteConstraints(50, 130, 510, 100));

        pack();
    }// </editor-fold>//GEN-END:initComponents





    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnGenerate;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JList<String> listPlayerInfo;
    private javax.swing.JScrollPane panelplayerInfo;
    private javax.swing.JLabel txtMessage;
    private javax.swing.JTextField txtNumberOfPlayers;
    // End of variables declaration//GEN-END:variables

/****************** here you may edit the source ******/
    /**
     * non GUI attributes
     */
    private static PokerController pc = null;
    private final ArrayList<IApplPoker> collApplPoker = new ArrayList<>();
    private final DefaultListModel<String> lmApplPoker = new DefaultListModel<>();
    private int currentPlayerIdx = 0;
    		
    		
    		
    
    /*
     *  methods to set next step in game
     */

    private void actionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_actionPerformed
        try {
            if (evt.getSource() == this.btnGenerate) {
            	int numOfPlayers = Integer.parseInt(txtNumberOfPlayers.getText());
            	for(int idx = 0; idx < numOfPlayers; idx++)
            	{
            		ApplPoker appl = new ApplPoker();
            		appl.setVisible(true);
            		collApplPoker.add(appl);
               		if(idx != 0)
            		{
            			collApplPoker.get(idx).setEnablePlayer(false);
            		}
            	}
            	refillList();
            }
        } catch (Exception ex) {
            txtMessage.setText("error: " + ex.getMessage());
        }
    }//GEN-LAST:event_actionPerformed

    
    /*
     * public methods to set next step in game
     * called by current player when dicing ends
     */
    public void setNextPlayer() {
    	collApplPoker.get(currentPlayerIdx).setEnablePlayer(false);
    	currentPlayerIdx++;
    	if(currentPlayerIdx >= collApplPoker.size())
    		currentPlayerIdx = 0;
    	
    	collApplPoker.get(currentPlayerIdx).setEnablePlayer(true);
    	try{
    		refillList();
    	}
    	catch(Exception ex)
    	{
    		txtMessage.setText(ex.getMessage());
    	}
    }
    /**
     * useful helper methods
     */
    private void refillList() throws Exception {
        lmApplPoker.clear();
        for(IApplPoker appl:this.collApplPoker) {
            lmApplPoker.addElement(appl.getNameOfPlayer() + ", score=" + appl.getTotalScoreOfPlayer());
        }
    }

    private void initOtherThings() {
        this.listPlayerInfo.setModel(lmApplPoker);
    }

}
