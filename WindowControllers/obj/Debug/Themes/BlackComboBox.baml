   M S B A M L   `   `   `  ����L  HWindowControllers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:WindowControllers.ThemesWindowControllers.Themes  5      ,    &WindowControllers.Themes.BlackComboBox   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:WindowControllers.Themes     5        � 	Resources  5	       �� 
TargetType �� Property �� Value ��Name ��Name �� Padding �� TypeInTargetAssembly �� 
ResourceId	 �� Triggers
 P� Property P� Value �� 
TargetName��%E  '��       0�� 5   }   *�� $ local:GenericL�$ TextBlockBlack��+10�� 5   W   *�� $ local:GenericL�$ FocusedTextBoxBackground��+10�� 5   P   *�� $ local:GenericL�$ FontColorFocused��+10�� 5   W   *�� $ local:GenericL�$ FocusedTextBoxBackground��+1�� 5
   
   " ��6   .�5      ��  � �� 5      �� 5      .R�5      �� $ 
ItemBorder��6!   
������ $��1q�6O   ._�5      �� $	 txt��6(   $��Center=�65   $��16(�6R   $ 3q�6q   ��6`   �� ��6}   2  5      	 5      P� 5      
 �@��
IsSelected . . �5   "   �� $ 
ItemBorder��6)    6W   2  � ���� 5   "   $	 txt��6)    6P   2  � ��5      P� 5      
 �@J�IsMouseOver . . �5   "   �� $ 
ItemBorder��6)    6W   2  � ��5      5      5      5      5      .��5      � $��1q�6   {�6   �� *IsComboBoxEnabled+ �� RelativeSource ��  �� AncestorType$ UserControlL�.+�5    
   �� ._�5!      � .+�5"      �� -   5        $ border��5"       �� CornerRadius$ 2��6)   $��1q�6:    J� SnapsToDevicePixels . ��5#      �� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+��5$      �� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+ J� Effect 5%      3  -System.Windows.Media.Effects.DropShadowEffect  5&         
BlurRadius$ 11Q�6+      Color$ Black��6;      Opacity$	 0.5Q�6I      ShadowDepth$ 1Q�6W   5(      �� 5)      -   5         ��Name$ ButtonSelect��5)      $��Stretch=�6/   
������ $��0q�6�   {�5*      �� *IsComboBoxEnabled+ �� $ UserControlL���5+      �� *�� $ local:GenericL�$ MainFontColor��+��5,      �� 5-       �� 
TargetType" ��6*   .R�5.      �� $
��Back��6%   $ 2��61   #��z��6B   #��z��6l   #��z��6�   ._�5/   "   �� $��Stretch=�63   $��Center=�6Q   50      	 51      P� 52   "   
 �@J�IsMouseOver . . �53   &   �� $
 Back��6-    6U   �� *�� $ local:GenericL�$ FocusedTextBoxBackground��+ � ��54   #   P� 55   "   
 �@J�IsMouseOver .  . �56   &   �� $
 Back��6-    6U   �� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+ � ��57   #   P� 58   "   
 � {� .  . �59   &   �� $
 Back��6-    6U   �� *�� $ local:GenericL�$ DisabledTextBoxBackground��+ � ��5:   #   5<      .��5>      �  � ColumnDefinitions 5?      �� 5@      $��10 �6/   �� 5A      $��1* �6/   �� 5B      $��20 �6/   .+�5D      �� -   5        $ SelectedValue��5D   $   $��16(�6;   $��Center=�6I   $��1��6f   ��6v   �� *�� $ local:GenericL�$ TextBlockBlack��+��5E      �� *SelectedItem+ �� Mode$ TwoWay=� �� $ UserControlL��� 5F      -   5        $	 Bar��5F   $   $��Center=�61   $��2��6N   $��Ʌ��5G      ��5F   ^   �� *�� $ local:GenericL�$ TextBlockBlack��+5G      5H      5I      5J      5K      � 5L   
   -   5         �Name$ popList��5L       � 	StaysOpen .  $��Slide=�5M   E    � AllowsTransparency . ��5L   3   ��  �� ElementName$ ButtonSelect�� �� Path$ ActualWidth� � PlacementTarget 5M      �� $ ButtonSelect��.��5N      � .+�5O      �� -   5        $ 	popBorder��5O      $ 1��6,   $	��2.5q�6=    . ��5P      �� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+��5Q      
� 5R        
� Center$  0.5,0.5&�6.   ! 
� RadiusX$! 1Q�6?   " 
� RadiusY$" 1Q�6K   .��5S      � # � Color$# 	#50000000��6+   $ � Offset$	$ 0.0Q�6=   � 5T      $# 	#15000000��6+   $	$ 0.3Q�6=   � 5U      $# 	#05000000��6+   $	$ 1.0Q�6=   5V      5X      �� 5Y      -   5        % ��Name$% VariantList��5]      
������ $��0q�5[   �   $��Stretch=�5\      ��5Y      �� *ItemsSource+ �� $ UserControlL���5Z      �� *�� $ local:GenericL�$ MainFontColor��+& �� SelectedItem& 5[      �� *SelectedItem+$ TwoWay=� �� $ UserControlL�5^      5_      5`      5a      5b      