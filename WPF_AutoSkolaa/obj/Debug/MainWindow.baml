   M S B A M L   `   `   `  ����I  EWPF_AutoSkolaa, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WPF_AutoSkolaaWPF_AutoSkolaa  5   	       WPF_AutoSkolaa.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WPF_AutoSkolaa     5   	     � Title$  	AutoSkola��5      $	��450��6   $	��800��6+   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5        $��dataGridCentralni��5
      $��Center=�6,   $��	0,217,0,0q�6J   $	��Top=�6]   $
��auto��5         
IsReadOnly .    AlternatingRowBackground
 ��� �   CanUserAddRows .     SelectionUnit$ FullRow=�6b      SelectionMode$ Single=�6z   �� 5   
   $
��Left=�6   $��30��60   $	��Top=�6=   $
��auto��6U    �� Orientation$ 
Horizontal=�6b   $��1��6{   $��	67,-2,0,0q�6�   .+�5      �� -   5         ��Name$ btnCas��5      $	��Cas��6%   $
��Left=�63   $��90��6N   $��	0,0,0,0.4q�6Z   6m   �� 5      -   5        $ btnInstruktor��5      $��
Instruktor��6,   $
��Left=�6A   $	��Top=�6]   $��30��6u   $��75��6�   6�   �� 5      -   5        $ btnIspit��5      $��Ispit��6'   $
��Left=�67   $	��Top=�6S   $��30��6k   $��75��6w   6�   �� 5      -   5        $ btnKandidat��5      $��Kandidat��6*   $
��Left=�6=   $	��Top=�6Y   $��30��6q   $��75��6}   6�   �� 5      -   5        $ btnObuka��5      $��Obuka��6'   $
��Left=�67   $	��Top=�6S   $��30��6k   $��75��6w   6�   �� 5      -   5        $ btnPrisustvo��5      $��	Prisustvo��6+   $
��Left=�6?   $	��Top=�6[   $��30��6s   $��75��6   6�   �� 5      -   5        $ 	btnUplata��5      $��Uplata��6(   $
��Left=�69   $	��Top=�6T   $��30��6l   $��75��6x   6�   �� 5      -	   5        $ 	btnVozilo��5      $��Vozilo��6(   $
��Left=�69   $	��Top=�6T   $��30��6l   $��75��6x   6�   5      �� 5   
   $
��Left=�6   $��30��60   $��Bottom=�6=   $
��auto��6X   $ 
Horizontal=�6e   $��1��6~   $��	193,0,0,0q�6�   .+�5      �� -
   5        $ btnDodaj��5      $��Dodaj��6'   $	��120��67   6C   �� 5      -   5        $ 	btnIzmeni��5      $��Izmeni��6(   $	��120��69   6E   �� 5      -   5        $ 	btnObrisi��5      $��Obrisi��6(   $	��120��69   6E   5      5      5      