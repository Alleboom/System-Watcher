   M S B A M L   `   `   `  ����N  JSystem Watcher MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:System_Watcher_MVVM.Helpers.Converters&System_Watcher_MVVM.Helpers.Converters  5      9    3System_Watcher_MVVM.Views.Lists.GroupsComputersView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Bl4clr-namespace:System_Watcher_MVVM.Helpers.Converters     5        � 	Resources  5	      @   :System_Watcher_MVVM.Helpers.Converters.DateToBoolConverter   conv��%   &	          5
   
   6    5      .��5      ��  �� Orientation$ 
Horizontal=�6   $��5q�6*   .+�5   
   �� $��0,0,5,0q�6   .+�5      �� $��Groups��6   �� 5      -   5         ��Name$ Groups��5      $
��Auto��6'    �� DisplayMemberPath$
 Name��64   �� 5      $ 
Horizontal=�6   .+�5      �� $��Owner: ��6   �� 5      -   5         ��Name$ SelectedGroupOwner��5      5      �� 5      $ 
Horizontal=�6   .+�5      �� $��
Location: ��6   �� 5      -   5        $ SelectedGroupLoc��5      5      �� 5      -   5         ��Name$ GenerateReportForAll��5   ;   $!��Generate report for all pcs��6   $��5q�6Y   �� 5      -   5        $ GenerateReportForGroup��5   A   $'��!Geneate report for selected group��6   $��5q�6a   5      � 5   
   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ 	Computers��5         AutoGenerateColumns .  "   VerticalScrollBarVisibility$ Visible=�6F   
���   �$��1q�6�   	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Name��6)      
IsReadOnly .    CanUserSort . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Name+6c     5!      $
 
Last Logon��6)    .  .  6=   �� *	LastLogon+6m   4  .System.Windows.Controls.DataGridCheckBoxColumn  5#      $
 Enabled?��6-    .  .  6d   �� *EnabledInAD+  5%      $
 Online?��6-    .  .  6d   �� *
IsOnline+��5+      �� 5,       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6    �� Triggers 5-      �� 5.       �� Binding 6*   �� *	LastLogon+ �� 	Converter# [   �� Value True6i   .a�5/   "   ��  �� Property � �� �� Value
 �  ��6?   50      52      54      55      56      57      