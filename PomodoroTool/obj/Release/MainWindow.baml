   M S B A M L   `   `   `  ����   PomodoroTool    PomodoroTool.MainWindow   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    -     �Name$  Window�� � Title$  ��$��121.25��$	��185�� � ShowInTaskbar .   � 
ResizeMode$ CanMinimize=� � Topmost .  � SizeToContent$ WidthAndHeight=� � WindowStyle$
 None=�$��Center=�$��Center=�   WindowTemplate#���   
������  � AllowsTransparency .  � 	Resources 	 �� 
TargetType
 J� SnapsToDevicePixels 
� GradientOrigin � Color � Offset �� 
TargetType �� Property �� Value��%G  &	        '��     �� "	 �.R�� 
 . .+�A� $��0,10,0,0q���
� $ 	0.75,0.25&�.��� $ Tomato��$	 0.0Q�� $	 Red��$	 0.5Q�� $ DarkRed��$	 1.0Q��� $6��0/PomodoroTool;component/Resources/tomatoStem.png��$��40��$��90��$	��Top=��� $��Center=�$��Center=��� " ��.���  � �� �� .��� $ 	OliveDrab��$	 0.2Q�� $ DarkOliveGreen��$	 0.5Q�� $ 	DarkGreen��$	 0.8Q���  � ��$ 6����  �@��StrokeEndLineCap$ Round=���  �@��StrokeLineJoin$ Round=���  �@��StrokeStartLineCap$ Triangle=� � ContextMenu �� .@��� -   $��	checkBox1��$	��120��.��常に手前に表示 d� 	IsChecked ��  �� ElementName$ 
MainWindow�� �� Path$ Topmost�� .+��� -   $��
MinuteText��$��30��.������ $ IntervalMinutes� �� UpdateSourceTrigger$ PropertyChanged=� �� StringFormat$ 00�� �� ValidationRules 5  /System.Windows.Controls.ExceptionValidationRule  .���� $��20��.��:�� -   $��
SecondText��$��30��.������ $ IntervalSeconds�$ PropertyChanged=�$ 00��   .���� -   $��SetTimeButton��.��Reset�� -   $��CloseButton��.��Exit.���� $��86.25��$��163.75��$��Center=�$��Stretch=�.+��� -   $��label2��$��Center=�$��Stretch=�$��Stretch=�$��Center=�
��� d �$
��Gigi)�$
��Bold"�$��
22,12,20,0q� � OverridesDefaultStyle .  ���� $ label1��$ FontSize����� $ label1��$ Content��� -   $��label1��$��50(�$��Center=�$��Stretch=�$��Stretch=�$��Center=�
���2͚�$
��Gigi)�$
��Bold"�$��
20,10,20,0q� .  .��Label