<!DOCTYPE qgis PUBLIC 'http://mrcc.com/qgis.dtd' 'SYSTEM'>
<qgis version="3.28.8-Firenze" labelsEnabled="1" styleCategories="LayerConfiguration|Symbology|Labeling" readOnly="0">
  <flags>
    <Identifiable>1</Identifiable>
    <Removable>1</Removable>
    <Searchable>1</Searchable>
    <Private>0</Private>
  </flags>
  <renderer-v2 enableorderby="0" type="pointCluster" tolerance="3" forceraster="0" symbollevels="0" toleranceUnitScale="3x:0,0,0,0,0,0" toleranceUnit="MM" referencescale="-1">
    <renderer-v2 enableorderby="0" type="RuleRenderer" forceraster="0" symbollevels="0" referencescale="-1">
      <rules key="{95a11461-b853-4a26-9b8c-4e067a3ec7b5}">
        <rule label="Emergency" symbol="0" scalemaxdenom="100000" key="{af946f90-0074-4259-8362-c11090c9f5ac}" scalemindenom="20000" filter="&quot;Category&quot; ~ 'Fire|Health'"/>
        <rule label="Fire" symbol="1" scalemaxdenom="20000" key="{ab07a800-3d4d-415f-9138-102cdb87319a}" scalemindenom="1" filter="&quot;Category&quot; = 'Fire'"/>
        <rule label="Health" symbol="2" scalemaxdenom="20000" key="{dc4a2388-b378-4678-a570-8287063e62f6}" scalemindenom="1" filter="&quot;Category&quot; = 'Health'"/>
        <rule label="Public" symbol="3" scalemaxdenom="100000" key="{342d57c5-647b-4395-b82f-fcbc2782d7f2}" scalemindenom="15000" filter="&quot;Category&quot; ~ 'Admin|Education|Public|Post(?!Box)'"/>
        <rule label="Education" symbol="4" scalemaxdenom="15000" key="{0b71e9d6-d851-4dd0-b645-69eba018b9c5}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Education'"/>
        <rule label="Government" symbol="5" scalemaxdenom="15000" key="{e3646b7b-94e7-4958-848a-801f12763215}" scalemindenom="1" filter="&quot;Category&quot; = 'Admin'"/>
        <rule label="Police" symbol="6" scalemaxdenom="15000" key="{fce03c4b-dd2a-4b5c-b04b-3333828f4ca0}" scalemindenom="1" filter="&quot;Category&quot; = 'Police'"/>
        <rule label="Post" symbol="7" scalemaxdenom="15000" key="{5d5837b8-ec09-48ff-97a2-1907bad8155d}" scalemindenom="1" filter="&quot;Category&quot; = 'Post'"/>
        <rule label="Parks" symbol="8" scalemaxdenom="20000" key="{c6832877-98d2-4a0a-8c6c-de3b5ec96cc2}" scalemindenom="7500" filter="&quot;Category&quot; ~ 'Attraction|MortuaryCemetery|Park(?!ing)'"/>
        <rule label="Attractions" symbol="9" scalemaxdenom="7500" key="{d36da2cf-4946-4b2e-a415-69798bf2ec48}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Attraction'"/>
        <rule label="Cemetery" symbol="10" scalemaxdenom="7500" key="{034eefd3-38e2-4377-a51d-ed0cc7587601}" scalemindenom="1" filter="&quot;Category&quot; = 'MortuaryCemetery'"/>
        <rule label="Parks" symbol="11" scalemaxdenom="7500" key="{adee621f-febb-4062-940e-fd201a3e523e}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Park(?!ing)'"/>
        <rule label="Utility" symbol="12" scalemaxdenom="15000" key="{078c5c37-a070-4f8b-9381-afd9c3954456}" scalemindenom="7500" filter="&quot;Category&quot; ~ 'Power|Sewage|Water|Waste'"/>
        <rule label="Power" symbol="13" scalemaxdenom="7500" key="{c8c8953c-43fa-4056-9e8c-3c8b86fa9a7f}" scalemindenom="1" filter="(&quot;Category&quot; ~ 'Power') AND NOT (&quot;Category&quot; ~ 'Waste')"/>
        <rule label="Sewage &amp; Water" symbol="14" scalemaxdenom="7500" key="{a80b9a3e-99df-4059-b6c4-6dccb7e5aecb}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Sewage|Water'"/>
        <rule label="Waste" symbol="15" scalemaxdenom="7500" key="{47134359-2cd0-44cd-9fa8-a9c21d8873bd}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Waste'"/>
        <rule label="Transportation" symbol="16" scalemaxdenom="100000" key="{5b79ead4-520b-4060-9722-d175002c13b1}" scalemindenom="30000" filter="&quot;Category&quot; ~ 'Building(Bus|PassengerTrain|PassengerShip|Subway|Tram)' AND NOT &quot;Category&quot; ~ 'Airplane'"/>
        <rule label="Bus / Tram Stops" symbol="17" scalemaxdenom="10000" key="{cebd7b9d-9ce4-4d9c-97b9-69426eb1752e}" scalemindenom="5000" filter="&quot;Category&quot; ~ '(Stop(?=Bus|Tram))'"/>
        <rule label="Bus Stops" symbol="18" scalemaxdenom="5000" key="{789f27b0-5244-45cf-82cd-0c28ffc602a3}" scalemindenom="1" filter="&quot;Category&quot; ~ 'StopBus'"/>
        <rule label="Tram Stops" symbol="19" scalemaxdenom="5000" key="{5091a73c-afc7-40dd-9eec-9371568d9765}" scalemindenom="1" filter="&quot;Category&quot; ~ 'StopTram'"/>
        <rule label="Bus Terminal" symbol="20" scalemaxdenom="30000" key="{c091717f-e369-49fb-bab8-46e87a53392d}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway|Train|Tram).)*BuildingBus((?!Airplane|Ship|Subway|Train|Tram).)*$'"/>
        <rule label="Harbor" symbol="21" scalemaxdenom="30000" key="{6f469667-c166-4500-8e41-52b48ca94d5b}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway|Train|Tram).)*BuildingPassengerShip((?!Airplane|Bus|Subway|Train|Tram).)*$'"/>
        <rule label="Train Station" symbol="22" scalemaxdenom="30000" key="{87f0a460-cdb6-4b06-897f-4366918aad80}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship|Subway|Tram).)*BuildingPassengerTrain((?!Airplane|Bus|Ship|Subway|Tram).)*$'"/>
        <rule label="Tram Station" symbol="23" scalemaxdenom="30000" key="{31498ba3-2f08-4c2b-9178-baf6db2e829d}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship|Subway|Train).)*BuildingTram((?!Airplane|Bus|Ship|Subway|Train).)*$'"/>
        <rule label="Subway Station" symbol="24" scalemaxdenom="30000" key="{86d339be-ce44-447a-b0f7-d371a9c69bce}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship|Train|Tram).)*BuildingSubway((?!Airplane|Bus|Ship|Train|Tram).)*$'"/>
        <rule label="Hubs" key="{be4f6e53-f040-40eb-a50c-a7087fb2320e}">
          <rule label="Bus &amp; Harbor" symbol="25" scalemaxdenom="30000" key="{35a007b3-3a6a-4efb-87af-4e503b22077e}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$'"/>
          <rule label="Bus &amp; Train" symbol="26" scalemaxdenom="30000" key="{009eb974-7469-4b44-a3cf-81a705618b0f}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$'"/>
          <rule label="Bus &amp; Subway" symbol="27" scalemaxdenom="30000" key="{03e3720f-cf80-420c-84e0-8dd4ae1ff2aa}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$'"/>
          <rule label="Harbor &amp; Train" symbol="28" scalemaxdenom="30000" key="{a0c12a36-5994-425d-843a-58a2bd0769de}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$'"/>
          <rule label="Harbor &amp; Subway" symbol="29" scalemaxdenom="30000" key="{ec28c4b2-46f7-4700-b09b-de564ae1feea}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$'"/>
          <rule label="Train &amp; Subway" symbol="30" scalemaxdenom="30000" key="{759290a1-318f-4ffe-a94c-7978edab0ca5}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'"/>
          <rule label="Bus, Harbor &amp; Train" symbol="31" scalemaxdenom="30000" key="{6e14ab23-7fdc-4afd-ae0a-7f5c81ec950a}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$'"/>
          <rule label="Bus, Harbor &amp; Subway" symbol="32" scalemaxdenom="30000" key="{8831103e-2648-47bc-a36b-c0339b9fee78}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$'"/>
          <rule label="Bus, Train &amp; Subway" symbol="33" scalemaxdenom="30000" key="{1eeb51d4-7c02-4f86-bf63-f48893401b2c}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$'"/>
          <rule label="Harbor, Train &amp; Subway" symbol="34" scalemaxdenom="30000" key="{36192762-803a-4896-928b-64f57fcab554}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'"/>
          <rule label="Bus, Harbor, Train &amp; Subway" symbol="35" scalemaxdenom="30000" key="{54046e85-89e0-4645-af9c-8a67dba218e3}" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane).)*BuildingBus((?!Airplane).)*$' AND &quot;Category&quot; ~ '^((?!Airplane).)*BuildingPassengerShip((?!Airplane).)*$' AND &quot;Category&quot; ~ '^((?!Airplane).)*BuildingPassengerTrain((?!Airplane).)*$' AND &quot;Category&quot; ~ '^((?!Airplane).)*BuildingSubway((?!Airplane).)*$'"/>
        </rule>
        <rule label="Road Services" symbol="36" scalemaxdenom="15000" key="{6115f3fc-f7ee-4cd4-b947-909ea06bd7fe}" scalemindenom="5000" filter="&quot;Category&quot; ~ 'StoreGasStation|Parking'"/>
        <rule label="Gas Station" symbol="37" scalemaxdenom="5000" key="{e241b10a-31b1-4b49-ba1c-f97dc83bdf1c}" scalemindenom="1" filter="&quot;Category&quot; = 'StoreGasStation'"/>
        <rule label="Parking" symbol="38" scalemaxdenom="5000" key="{2c8dd178-32f2-46cf-ba1d-3b1230c97886}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Parking'"/>
        <rule label="Catering" symbol="39" scalemaxdenom="15000" key="{98f92b1c-7bd7-4679-be5a-173effb41dcd}" scalemindenom="5000" filter="&quot;Category&quot; ~ 'Store(Bar|Beverage|Food|Restaurant)'"/>
        <rule label="Bar" symbol="40" scalemaxdenom="5000" key="{1f739a6c-cbad-4f45-bb42-7288e78d4b6c}" scalemindenom="1" filter="&quot;Category&quot; = 'StoreBar'"/>
        <rule label="Beverage" symbol="41" scalemaxdenom="5000" key="{245ab92f-8bdd-4985-ad60-fb385ecb1d5f}" scalemindenom="1" filter="&quot;Category&quot; = 'StoreBeverage'"/>
        <rule label="Restaurant" symbol="42" scalemaxdenom="5000" key="{b079b650-fa58-48b9-be7f-feba9bab7777}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Store(Food|Restaurant)'"/>
        <rule label="Other Services" symbol="43" scalemaxdenom="15000" key="{ec77882f-925b-412a-a7d1-a5794ee1ea02}" scalemindenom="5000" filter="&quot;Category&quot; ~ 'Store(Bank|ConvenienceStore|Hotel)'"/>
        <rule label="Bank" symbol="44" scalemaxdenom="5000" key="{259a1b98-b0e7-457b-ac59-dc494a7a2dc9}" scalemindenom="1" filter="&quot;Category&quot; = 'StoreBank'"/>
        <rule label="Convenience Store" symbol="45" scalemaxdenom="5000" key="{47415ebd-c90f-4932-9c39-2dcce4bc316c}" scalemindenom="1" filter="&quot;Category&quot; = 'StoreConvenienceStore'"/>
        <rule label="Hotel" symbol="46" scalemaxdenom="5000" key="{f27d588b-3d72-45aa-b6a9-b0ee52dcf6ef}" scalemindenom="1" filter="&quot;Category&quot; = 'StoreHotel'"/>
        <rule label="Traffic Light" symbol="47" scalemaxdenom="4000" key="{0af246c0-d8ee-477a-a370-2fd5ac9a93b8}" scalemindenom="1" filter="&quot;Category&quot; = 'TrafficLight'"/>
      </rules>
      <symbols>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="0" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,90,90,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="1" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,90,90,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="emergency/amenity=fire_station.svg" name="name"/>
              <Option type="QString" value="-0.10000000000000001,-0.10000000000000001" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="10" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="80,180,60,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="🪦" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Bold" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="-0.10000000000000001,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="11" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="80,180,60,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="gpsicons/tree.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="255,255,255,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="12" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="50,150,150,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="13" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="50,150,150,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="⚡" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="0,-0.40000000000000008" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="14" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="50,150,150,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="💧" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="0,-0.29999999999999999" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="15" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="50,150,150,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="amenity/amenity_recycling.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="16" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" value="false" name="active"/>
                    <Option type="int" value="1" name="type"/>
                    <Option type="QString" value="" name="val"/>
                  </Option>
                </Option>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="17" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" value="false" name="active"/>
                    <Option type="int" value="1" name="type"/>
                    <Option type="QString" value="" name="val"/>
                  </Option>
                </Option>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="18" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" value="false" name="active"/>
                    <Option type="int" value="1" name="type"/>
                    <Option type="QString" value="" name="val"/>
                  </Option>
                </Option>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.87419" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="enabled">
                    <Option type="bool" value="false" name="active"/>
                    <Option type="int" value="1" name="type"/>
                    <Option type="QString" value="" name="val"/>
                  </Option>
                </Option>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="19" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" value="false" name="active"/>
                    <Option type="int" value="1" name="type"/>
                    <Option type="QString" value="" name="val"/>
                  </Option>
                </Option>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/railway=station.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.8" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="enabled">
                    <Option type="bool" value="false" name="active"/>
                    <Option type="int" value="1" name="type"/>
                    <Option type="QString" value="" name="val"/>
                  </Option>
                </Option>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="2" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,90,90,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="cross_fill" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="20" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="21" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="22" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="23" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/railway=station.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="24" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="0,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="25" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="26" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="27" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="2,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="28" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="29" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="1.99999999999999978,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="2,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-1.99999999999999978,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="-1.99999999999999978,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="3" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,160,100,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="30" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="1.99999999999999978,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="2,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-1.99999999999999978,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="-1.99999999999999978,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="31" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="32" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="4,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="33" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="4,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="34" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="4,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="-4,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="35" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="6,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="FontMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="M" name="chr"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="Segoe UI" name="font"/>
              <Option type="QString" value="Black" name="font_style"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="miter" name="joinstyle"/>
              <Option type="QString" value="6,-0.5" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_train_station2.svg" name="name"/>
              <Option type="QString" value="2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_port.svg" name="name"/>
              <Option type="QString" value="-2,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="20,145,230,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="rounded_square" name="name"/>
              <Option type="QString" value="-6,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.5" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="2" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_bus_stop.svg" name="name"/>
              <Option type="QString" value="-6,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="36" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="130,150,240,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="37" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="130,150,240,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="gpsicons/gas.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="255,255,255,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="38" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="130,150,240,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="transport/transport_parking.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="39" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,100,255,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="4" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,160,100,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="amenity/amenity_library.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="40" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,100,255,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="food/food_bar.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="255,255,255,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="41" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,100,255,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="food/food_pub.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="255,255,255,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="42" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,100,255,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="food/food_restaurant.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="255,255,255,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="43" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,120,200,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="44" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,120,200,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="gpsicons/bank.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="255,255,255,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="45" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,120,200,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="shopping/shopping_convenience.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="255,255,255,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="46" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,120,200,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="services/tourism=hotel.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="47" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="245,245,245,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,1.19999999999999996" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="78,78,78,255" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.1" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="243,243,243,0" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="half_square" name="name"/>
              <Option type="QString" value="0.90000000000000002,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="91,84,84,255" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.1" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="244,244,244,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,-1.19999999999999996" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="78,78,78,255" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.1" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="245,245,245,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="half_square" name="name"/>
              <Option type="QString" value="0.90000000000000002,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="78,78,78,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="60,180,60,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,1.19999999999999996" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,200,0,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,-0.00000000000000006" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,0,0,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,-1.19999999999999996" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="5" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,160,100,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="tourist/tourist_museum.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="6" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,160,100,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="amenity/amenity_police.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="7" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="200,160,100,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="amenity/amenity_post_box.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.6" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="8" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="80,180,60,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="9" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="2" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="80,180,60,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="26,115,232,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0.3" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="shopping/shopping_photo.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.75" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
      </symbols>
    </renderer-v2>
    <symbol is_animated="0" type="marker" clip_to_extent="1" name="centerSymbol" frame_rate="10" alpha="1" force_rhr="0">
      <data_defined_properties>
        <Option type="Map">
          <Option type="QString" value="" name="name"/>
          <Option name="properties"/>
          <Option type="QString" value="collection" name="type"/>
        </Option>
      </data_defined_properties>
      <layer pass="0" enabled="0" locked="0" class="SimpleMarker">
        <Option type="Map">
          <Option type="QString" value="0" name="angle"/>
          <Option type="QString" value="square" name="cap_style"/>
          <Option type="QString" value="245,75,80,0" name="color"/>
          <Option type="QString" value="1" name="horizontal_anchor_point"/>
          <Option type="QString" value="bevel" name="joinstyle"/>
          <Option type="QString" value="circle" name="name"/>
          <Option type="QString" value="0,0" name="offset"/>
          <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
          <Option type="QString" value="MM" name="offset_unit"/>
          <Option type="QString" value="35,35,35,0" name="outline_color"/>
          <Option type="QString" value="solid" name="outline_style"/>
          <Option type="QString" value="0" name="outline_width"/>
          <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
          <Option type="QString" value="MM" name="outline_width_unit"/>
          <Option type="QString" value="diameter" name="scale_method"/>
          <Option type="QString" value="1" name="size"/>
          <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
          <Option type="QString" value="MM" name="size_unit"/>
          <Option type="QString" value="1" name="vertical_anchor_point"/>
        </Option>
        <data_defined_properties>
          <Option type="Map">
            <Option type="QString" value="" name="name"/>
            <Option name="properties"/>
            <Option type="QString" value="collection" name="type"/>
          </Option>
        </data_defined_properties>
      </layer>
    </symbol>
  </renderer-v2>
  <labeling type="rule-based">
    <rules key="{fa8a483f-d3ba-4c00-b709-9ca3f7b4f637}">
      <rule scalemaxdenom="7500" description="Park" key="{0745966f-66fb-449b-a15e-f4def008f007}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Attraction|MortuaryCemetery|Park(?!ing)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="80,180,60,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Pk" textOpacity="1" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="1" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if(@id % 2 = 0, 5, 3)" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule scalemaxdenom="20000" description="Emergency" key="{8a038d27-f676-4f8a-93a6-b3f5a0f32043}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Fire|Health'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="239,144,144,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Em" textOpacity="1" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="1" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if(@id % 2 = 0, 5, 3)" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule scalemaxdenom="15000" description="Government" key="{8231d0a6-7e35-4eac-abd2-2fc27ab33897}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Admin|Education|Police|Post(?!Box)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="162,137,88,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Gv" textOpacity="1" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="0" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="AutoWrapLength">
                  <Option type="bool" value="false" name="active"/>
                  <Option type="int" value="1" name="type"/>
                  <Option type="QString" value="" name="val"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if(@id % 2 = 0, 5, 3)" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule scalemaxdenom="2500" description="Bus / Tram Stops" key="{23d0145b-47e3-4913-8fd6-682fb882539a}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Stop(?=Bus|Tram)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="20,145,230,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="8" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Tr" textOpacity="1" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="0" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="2.5" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="AutoWrapLength">
                  <Option type="bool" value="false" name="active"/>
                  <Option type="int" value="1" name="type"/>
                  <Option type="QString" value="" name="val"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if(@id % 2 = 0, 5, 3)" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule scalemaxdenom="30000" description="Transportation" key="{0619ca59-aae1-400d-94f1-3c34d797f4f3}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Building(Bus|PassengerShip|PassengerTrain|Subway|Tram)' AND NOT &quot;Category&quot; ~ 'Airplane'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="20,145,230,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Tr" textOpacity="1" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="1" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="-3" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="LabelDistance">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if ((&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'), 5, if ((&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'), 7, 3))" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="5" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule scalemaxdenom="4000" description="Stores" key="{d0cf27e0-1a75-44e7-b440-188e50aca958}" scalemindenom="1" filter="&quot;Category&quot; ~ '(?:Store(?:Bank|Bar|Beverage|ConvenienceStore|Food|GasStation|Hotel|Restaurant)|Parking)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="0,0,0,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="St" textOpacity="1" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="1" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="Color">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if(&quot;Category&quot; ~ 'Store(Bank|ConvenienceStore|Hotel)', '#ff78c8', if(&quot;Category&quot; ~ 'Store(Bar|Beverage|Food|Restaurant)', '#c864ff', if(&quot;Category&quot; ~ 'StoreGasStation|Parking', '#8296f0', '#ff78c8')))" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="5" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule scalemaxdenom="7500" description="Utility" key="{262278fb-abb6-4a5c-8e29-7cbd0994c6e0}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Power|Sewage|Waste|Water'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="50,150,150,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Ut" textOpacity="1" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="1" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="-3" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="LabelDistance">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if ((&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'), 5, if ((&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'), 7, 3))" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="5" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{0dc4fbf7-730e-47e4-a83f-980f53dc47de}" filter="ELSE">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="255,255,255,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="1" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Semibold" fieldName="0" capitalization="0" legendString="Aa" textOpacity="0" fontWeight="63">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="0" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="1" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="1" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="0" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="4" repeatDistanceUnits="MM" placement="1" priority="5" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="0" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
    </rules>
  </labeling>
  <blendMode>0</blendMode>
  <featureBlendMode>0</featureBlendMode>
  <previewExpression>"Category"</previewExpression>
  <layerGeometryType>0</layerGeometryType>
</qgis>
