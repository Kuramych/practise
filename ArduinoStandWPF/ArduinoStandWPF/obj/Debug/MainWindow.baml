   M S B A M L   `   `   `  ����J  FArduinoStandWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ArduinoStandWPFArduinoStandWPF  5   	        ArduinoStandWPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:ArduinoStandWPF     5   	     � Title$  Arduino stand��5   	   $	��700��6   $��967.2��6,    � Icon$ Icon.ico��6:    � WindowState$ 	Maximized=�6J   .��5	      � -   5         �Name$
 grid��5	       � RowDefinitions 5
   
   �� 5      $	��60* �6   �� 5      $��7* �6    � ColumnDefinitions 5   
   �� 5      $
��417* �6   �� 5      $
��376* �6   .+�5   
   �� -   5         ��Name$ canvas��5      $��0,30,207.8,0q�6!   $��2��67   $	��570��6h   $	��Top=�6u   �� 5   
   -   5         ��Name$
 conf��5      $��2��6   $$��КОНФИГУРИРОВАТЬ��63   $��Center=�6M   $��0,18,0,16.4q�6j   $��Center=�6   $	��148��6�   $��36��6�   $��1��6�   c� 5   
   $��30��6   $��0q�6   $	��Top=�6)   $
��Left=�6A   $��89��6\   .@�5      �� -   5         ��Name$ color_changing��5      $��26��6/   $��
0,0,0,-0.4q�6;   $	��Top=�6O   $
��37.5��6g   	 �� SelectedIndex$	 0��6t   .@�5      �� -   5        
 ��Name$
 green��5      $��26��6.   $
��Left=�6:   $��35��6U   .��5      �� $��green (3).png��6   $��22��63   $��22��6>   5      �� 5      -   5        $	
 red��5      $��26��6,   $
��Left=�68   $��35��6S   .��5      �� $��red.png��6   $��22��6-   $��26��68   5      �� 5      -   5        $

 blue��5      $��26��6-   $
��Left=�69   $��35��6T   .��5      �� $��blue.png��6   $��22��6.   $��26��69   5      �� 5      -   5        $
 yellow��5      $��26��6/   $
��Left=�6;   $��35��6V   .��5       �� $��
yellow.png��6   $��22��60   $��26��6;   5!      �� 5"      -	   5        $
 purple��5"      $��26��6/   $
��Left=�6;   $��35��6V   .��5#      �� $��
purple.png��6   $��22��60   $��26��6;   5$      �� 5%      -
   5        $
 black��5%      $��26��6.   $
��Left=�6:   $��35��6U   .��5&      �� $��	black.png��6   $��22��6/   $��26��6:   5'      5)      �� 5*      -   5        $ delete��5*      $	��Top=�6%   $��26��6=   $��
0,0,0,-0.4q�6H   $��26��6\   .��5+      �� $��
delete.ico��6   5,      5-      � 5.   
   -   5         �Name$ 
components��5.      $��1��6(   $��Right=�68   $	��570��6T   $��255.4,30,-0.2,0q�6a   $	��Top=�6z   $	��200��6�   �� 5/   
   $��Компоненты��6   $��1��6%   $��Right=�65   $��
0,0,-0.2,0q�6Q   $	��Top=�6e   $	��200��6}   $��30��6�   $��14(�6�   $
��Bold"�6�   $��Center=�6�   $��Bottom=�6�   51      52      