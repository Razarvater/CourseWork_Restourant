   M S B A M L   `   `   `  ���� L  HWindowControllers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:WindowControllers.ThemesWindowControllers.Themes  ,    &WindowControllers.Themes.BlackComboBox   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     8local&clr-namespace:WindowControllers.Themes       � 	Resources   �� 
TargetType �� Property �� Value ��Name ��Name �� Padding �� TypeInTargetAssembly �� 
ResourceId	 �� Triggers
 P� Property P� Value �� 
TargetName��%4  '��       0�� *�� $ local:GenericL�$ TextBlockBlack��+10�� *�� $ local:GenericL�$ FocusedTextBoxBackground��+10�� *�� $ local:GenericL�$ FontColorFocused��+10�� *�� $ local:GenericL�$ FocusedTextBoxBackground��+1�� " ��.���  � �� �� .R��� $ 
ItemBorder��
������ $��1q�._��� $	 txt��$��Center=�$��16(�$ 3q����� ��2  	 P� 
 �@��
IsSelected . . ��� $ 
ItemBorder�� 2  � ���� $	 txt�� 2  � ��P� 
 �@J�IsMouseOver . . ��� $ 
ItemBorder�� 2  � ��.��� $��1q�{��� *IsComboBoxEnabled+ �� RelativeSource ��  �� AncestorType$ UserControlL�.+��� ._�� .+��� -   $ border�� �� CornerRadius$ 2��$��1q� J� SnapsToDevicePixels . ���� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+���� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+ J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 11Q�   Color$ Black��   Opacity$	 0.5Q�   ShadowDepth$ 1Q��� -    ��Name$ ButtonSelect��$��Stretch=�
������ $��0q�{��� *IsComboBoxEnabled+ �� $ UserControlL����� *�� $ local:GenericL�$ MainFontColor��+����  �� 
TargetType" ��.R��� $
��Back��$ 2��#��z��#��z��#��z��._��� $��Stretch=�$��Center=�	 P� 
 �@J�IsMouseOver . . ��� $
 Back�� �� *�� $ local:GenericL�$ FocusedTextBoxBackground��+ � ��P� 
 �@J�IsMouseOver .  . ��� $
 Back�� �� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+ � ��P� 
 � {� .  . ��� $
 Back�� �� *�� $ local:GenericL�$ DisabledTextBoxBackground��+ � ��.���  � ColumnDefinitions �� $��10 ��� $��1* ��� $��20 �.+��� -   $ SelectedValue��$��16(�$��Center=�$��1������ *�� $ local:GenericL�$ TextBlockBlack��+���� *SelectedItem+ �� Mode$ TwoWay=� �� $ UserControlL��� -   $	 Bar��$��Center=�$��2��$��Ʌ������ *�� $ local:GenericL�$ TextBlockBlack��+� -    �Name$ popList�� � 	StaysOpen .  $��Slide=� � AllowsTransparency . ����  �� ElementName$ ButtonSelect�� �� Path$ ActualWidth� � PlacementTarget �� $ ButtonSelect��.��� .+��� -   $ 	popBorder��$ 1��$	��2.5q� . ���� *�� $ local:GenericL�$! NotFocusedTextBoxBackground��+��
�   
� Center$  0.5,0.5&�! 
� RadiusX$! 1Q�" 
� RadiusY$" 1Q�.��� # � Color$# 	#50000000��$ � Offset$	$ 0.0Q�� $# 	#15000000��$	$ 0.3Q�� $# 	#05000000��$	$ 1.0Q��� -   % ��Name$% VariantList��
������ $��0q�$��Stretch=����� *ItemsSource+ �� $ UserControlL����� *�� $ local:GenericL�$ MainFontColor��+& �� SelectedItem& �� *SelectedItem+$ TwoWay=� �� $ UserControlL�