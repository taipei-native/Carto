<!DOCTYPE qgis PUBLIC 'http://mrcc.com/qgis.dtd' 'SYSTEM'>
<qgis readOnly="0" styleCategories="LayerConfiguration|Symbology|Labeling" labelsEnabled="1" version="3.28.8-Firenze">
  <flags>
    <Identifiable>1</Identifiable>
    <Removable>1</Removable>
    <Searchable>1</Searchable>
    <Private>0</Private>
  </flags>
  <renderer-v2 tolerance="3" toleranceUnitScale="3x:0,0,0,0,0,0" type="pointCluster" referencescale="-1" forceraster="0" enableorderby="0" toleranceUnit="MM" symbollevels="0">
    <renderer-v2 type="RuleRenderer" referencescale="-1" forceraster="0" enableorderby="0" symbollevels="0">
      <rules key="{95a11461-b853-4a26-9b8c-4e067a3ec7b5}">
        <rule label="Emergency" key="{af946f90-0074-4259-8362-c11090c9f5ac}" symbol="0" scalemaxdenom="100000" scalemindenom="20000" filter="&quot;Category&quot; ~ 'Fire|Health'"/>
        <rule label="Fire" key="{ab07a800-3d4d-415f-9138-102cdb87319a}" symbol="1" scalemaxdenom="20000" scalemindenom="1" filter="&quot;Category&quot; = 'Fire'"/>
        <rule label="Health" key="{dc4a2388-b378-4678-a570-8287063e62f6}" symbol="2" scalemaxdenom="20000" scalemindenom="1" filter="&quot;Category&quot; = 'Health'"/>
        <rule label="Public" key="{342d57c5-647b-4395-b82f-fcbc2782d7f2}" symbol="3" scalemaxdenom="100000" scalemindenom="15000" filter="&quot;Category&quot; ~ 'Admin|Education|Public|Post(?!Box)'"/>
        <rule label="Education" key="{0b71e9d6-d851-4dd0-b645-69eba018b9c5}" symbol="4" scalemaxdenom="15000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Education'"/>
        <rule label="Government" key="{e3646b7b-94e7-4958-848a-801f12763215}" symbol="5" scalemaxdenom="15000" scalemindenom="1" filter="&quot;Category&quot; = 'Admin'"/>
        <rule label="Police" key="{fce03c4b-dd2a-4b5c-b04b-3333828f4ca0}" symbol="6" scalemaxdenom="15000" scalemindenom="1" filter="&quot;Category&quot; = 'Police'"/>
        <rule label="Post" key="{5d5837b8-ec09-48ff-97a2-1907bad8155d}" symbol="7" scalemaxdenom="15000" scalemindenom="1" filter="&quot;Category&quot; = 'Post'"/>
        <rule label="Parks" key="{c6832877-98d2-4a0a-8c6c-de3b5ec96cc2}" symbol="8" scalemaxdenom="20000" scalemindenom="7500" filter="&quot;Category&quot; ~ 'Attraction|MortuaryCemetery|Park(?!ing)'"/>
        <rule label="Attractions" key="{d36da2cf-4946-4b2e-a415-69798bf2ec48}" symbol="9" scalemaxdenom="7500" scalemindenom="1" filter="&quot;Category&quot; ~ 'Attraction'"/>
        <rule label="Cemetery" key="{034eefd3-38e2-4377-a51d-ed0cc7587601}" symbol="10" scalemaxdenom="7500" scalemindenom="1" filter="&quot;Category&quot; = 'MortuaryCemetery'"/>
        <rule label="Parks" key="{adee621f-febb-4062-940e-fd201a3e523e}" symbol="11" scalemaxdenom="7500" scalemindenom="1" filter="&quot;Category&quot; ~ 'Park(?!ing)'"/>
        <rule label="Utility" key="{078c5c37-a070-4f8b-9381-afd9c3954456}" symbol="12" scalemaxdenom="15000" scalemindenom="7500" filter="&quot;Category&quot; ~ 'Power|Sewage|Water|Waste'"/>
        <rule label="Power" key="{c8c8953c-43fa-4056-9e8c-3c8b86fa9a7f}" symbol="13" scalemaxdenom="7500" scalemindenom="1" filter="(&quot;Category&quot; ~ 'Power') AND NOT (&quot;Category&quot; ~ 'Waste')"/>
        <rule label="Sewage &amp; Water" key="{a80b9a3e-99df-4059-b6c4-6dccb7e5aecb}" symbol="14" scalemaxdenom="7500" scalemindenom="1" filter="&quot;Category&quot; ~ 'Sewage|Water'"/>
        <rule label="Waste" key="{47134359-2cd0-44cd-9fa8-a9c21d8873bd}" symbol="15" scalemaxdenom="7500" scalemindenom="1" filter="&quot;Category&quot; ~ 'Waste'"/>
        <rule label="Transportation" key="{5b79ead4-520b-4060-9722-d175002c13b1}" symbol="16" scalemaxdenom="100000" scalemindenom="30000" filter="&quot;Category&quot; ~ 'Building(Bus|PassengerTrain|PassengerShip|Subway|Tram)' AND NOT &quot;Category&quot; ~ 'Airplane'"/>
        <rule label="Bus / Tram Stops" key="{cebd7b9d-9ce4-4d9c-97b9-69426eb1752e}" symbol="17" scalemaxdenom="10000" scalemindenom="5000" filter="&quot;Category&quot; ~ '(Stop(?=Bus|Tram))'"/>
        <rule label="Bus Stops" key="{789f27b0-5244-45cf-82cd-0c28ffc602a3}" symbol="18" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; ~ 'StopBus'"/>
        <rule label="Tram Stops" key="{5091a73c-afc7-40dd-9eec-9371568d9765}" symbol="19" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; ~ 'StopTram'"/>
        <rule label="Airport" key="{7f95b426-66fb-4ceb-a668-dede2fb3fc85}" symbol="20" scalemaxdenom="100000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Building(Cargo|Passenger)Airplane'"/>
        <rule label="Bus Terminal" key="{c091717f-e369-49fb-bab8-46e87a53392d}" symbol="21" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway|Train|Tram).)*BuildingBus((?!Airplane|Ship|Subway|Train|Tram).)*$'"/>
        <rule label="Harbor" key="{6f469667-c166-4500-8e41-52b48ca94d5b}" symbol="22" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway|Train|Tram).)*BuildingPassengerShip((?!Airplane|Bus|Subway|Train|Tram).)*$'"/>
        <rule label="Train Station" key="{87f0a460-cdb6-4b06-897f-4366918aad80}" symbol="23" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship|Subway|Tram).)*BuildingPassengerTrain((?!Airplane|Bus|Ship|Subway|Tram).)*$'"/>
        <rule label="Tram Station" key="{31498ba3-2f08-4c2b-9178-baf6db2e829d}" symbol="24" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship|Subway|Train).)*BuildingTram((?!Airplane|Bus|Ship|Subway|Train).)*$'"/>
        <rule label="Subway Station" key="{86d339be-ce44-447a-b0f7-d371a9c69bce}" symbol="25" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship|Train|Tram).)*BuildingSubway((?!Airplane|Bus|Ship|Train|Tram).)*$'"/>
        <rule label="Hubs" key="{be4f6e53-f040-40eb-a50c-a7087fb2320e}">
          <rule label="Bus &amp; Harbor" key="{35a007b3-3a6a-4efb-87af-4e503b22077e}" symbol="26" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$'"/>
          <rule label="Bus &amp; Train" key="{009eb974-7469-4b44-a3cf-81a705618b0f}" symbol="27" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$'"/>
          <rule label="Bus &amp; Subway" key="{03e3720f-cf80-420c-84e0-8dd4ae1ff2aa}" symbol="28" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$'"/>
          <rule label="Harbor &amp; Train" key="{a0c12a36-5994-425d-843a-58a2bd0769de}" symbol="29" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$'"/>
          <rule label="Harbor &amp; Subway" key="{ec28c4b2-46f7-4700-b09b-de564ae1feea}" symbol="30" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$'"/>
          <rule label="Train &amp; Subway" key="{759290a1-318f-4ffe-a94c-7978edab0ca5}" symbol="31" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'"/>
          <rule label="Bus, Harbor &amp; Train" key="{6e14ab23-7fdc-4afd-ae0a-7f5c81ec950a}" symbol="32" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$'"/>
          <rule label="Bus, Harbor &amp; Subway" key="{8831103e-2648-47bc-a36b-c0339b9fee78}" symbol="33" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$'"/>
          <rule label="Bus, Train &amp; Subway" key="{1eeb51d4-7c02-4f86-bf63-f48893401b2c}" symbol="34" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$'"/>
          <rule label="Harbor, Train &amp; Subway" key="{36192762-803a-4896-928b-64f57fcab554}" symbol="35" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'"/>
          <rule label="Bus, Harbor, Train &amp; Subway" key="{54046e85-89e0-4645-af9c-8a67dba218e3}" symbol="36" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ '^((?!Airplane).)*BuildingBus((?!Airplane).)*$' AND &quot;Category&quot; ~ '^((?!Airplane).)*BuildingPassengerShip((?!Airplane).)*$' AND &quot;Category&quot; ~ '^((?!Airplane).)*BuildingPassengerTrain((?!Airplane).)*$' AND &quot;Category&quot; ~ '^((?!Airplane).)*BuildingSubway((?!Airplane).)*$'"/>
        </rule>
        <rule label="Road Services" key="{6115f3fc-f7ee-4cd4-b947-909ea06bd7fe}" symbol="37" scalemaxdenom="15000" scalemindenom="5000" filter="&quot;Category&quot; ~ 'StoreGasStation|Parking'"/>
        <rule label="Gas Station" key="{e241b10a-31b1-4b49-ba1c-f97dc83bdf1c}" symbol="38" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; = 'StoreGasStation'"/>
        <rule label="Parking" key="{2c8dd178-32f2-46cf-ba1d-3b1230c97886}" symbol="39" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Parking'"/>
        <rule label="Catering" key="{98f92b1c-7bd7-4679-be5a-173effb41dcd}" symbol="40" scalemaxdenom="15000" scalemindenom="5000" filter="&quot;Category&quot; ~ 'Store(Bar|Beverage|Food|Restaurant)'"/>
        <rule label="Bar" key="{1f739a6c-cbad-4f45-bb42-7288e78d4b6c}" symbol="41" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; = 'StoreBar'"/>
        <rule label="Beverage" key="{245ab92f-8bdd-4985-ad60-fb385ecb1d5f}" symbol="42" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; = 'StoreBeverage'"/>
        <rule label="Restaurant" key="{b079b650-fa58-48b9-be7f-feba9bab7777}" symbol="43" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Store(Food|Restaurant)'"/>
        <rule label="Other Services" key="{ec77882f-925b-412a-a7d1-a5794ee1ea02}" symbol="44" scalemaxdenom="15000" scalemindenom="5000" filter="&quot;Category&quot; ~ 'Store(Bank|ConvenienceStore|Hotel)'"/>
        <rule label="Bank" key="{259a1b98-b0e7-457b-ac59-dc494a7a2dc9}" symbol="45" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; = 'StoreBank'"/>
        <rule label="Convenience Store" key="{47415ebd-c90f-4932-9c39-2dcce4bc316c}" symbol="46" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; = 'StoreConvenienceStore'"/>
        <rule label="Hotel" key="{f27d588b-3d72-45aa-b6a9-b0ee52dcf6ef}" symbol="47" scalemaxdenom="5000" scalemindenom="1" filter="&quot;Category&quot; = 'StoreHotel'"/>
        <rule label="Traffic Light" key="{0af246c0-d8ee-477a-a370-2fd5ac9a93b8}" symbol="48" scalemaxdenom="4000" scalemindenom="1" filter="&quot;Category&quot; = 'TrafficLight'"/>
      </rules>
      <symbols>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="0" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,90,90,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="1" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,90,90,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="1" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="emergency/amenity=fire_station.svg"/>
              <Option type="QString" name="offset" value="-0.10000000000000001,-0.10000000000000001"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="10" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="80,180,60,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="🪦"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Bold"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="-0.10000000000000001,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="11" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="80,180,60,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="gpsicons/tree.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="255,255,255,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="12" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="50,150,150,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="13" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="50,150,150,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="⚡"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="0,-0.40000000000000008"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="14" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="50,150,150,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="💧"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="0,-0.29999999999999999"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="15" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="50,150,150,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="amenity/amenity_recycling.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="16" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" name="active" value="false"/>
                    <Option type="int" name="type" value="1"/>
                    <Option type="QString" name="val" value=""/>
                  </Option>
                </Option>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="17" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" name="active" value="false"/>
                    <Option type="int" name="type" value="1"/>
                    <Option type="QString" name="val" value=""/>
                  </Option>
                </Option>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="18" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" name="active" value="false"/>
                    <Option type="int" name="type" value="1"/>
                    <Option type="QString" name="val" value=""/>
                  </Option>
                </Option>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="1" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.87419"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="enabled">
                    <Option type="bool" name="active" value="false"/>
                    <Option type="int" name="type" value="1"/>
                    <Option type="QString" name="val" value=""/>
                  </Option>
                </Option>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="19" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="size">
                    <Option type="bool" name="active" value="false"/>
                    <Option type="int" name="type" value="1"/>
                    <Option type="QString" name="val" value=""/>
                  </Option>
                </Option>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="1" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/railway=station.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.8"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option type="Map" name="properties">
                  <Option type="Map" name="enabled">
                    <Option type="bool" name="active" value="false"/>
                    <Option type="int" name="type" value="1"/>
                    <Option type="QString" name="val" value=""/>
                  </Option>
                </Option>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="2" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,90,90,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="cross_fill"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="20" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_airport2.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="21" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="22" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="23" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="24" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/railway=station.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="25" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="0,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="26" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="27" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="28" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="2,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="29" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="3" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,160,100,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="30" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="1.99999999999999978,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="offset" value="2,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-1.99999999999999978,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="-1.99999999999999978,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="31" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="1.99999999999999978,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="offset" value="2,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-1.99999999999999978,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="-1.99999999999999978,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="32" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="33" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="4,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="34" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="4,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="35" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="4,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="-4,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="36" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="6,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="FontMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="chr" value="M"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="font" value="Segoe UI"/>
              <Option type="QString" name="font_style" value="Black"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="miter"/>
              <Option type="QString" name="offset" value="6,-0.5"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_train_station2.svg"/>
              <Option type="QString" name="offset" value="2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_port.svg"/>
              <Option type="QString" name="offset" value="-2,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="20,145,230,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="rounded_square"/>
              <Option type="QString" name="offset" value="-6,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.5"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="2" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_bus_stop.svg"/>
              <Option type="QString" name="offset" value="-6,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="37" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="130,150,240,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="38" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="130,150,240,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="gpsicons/gas.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="255,255,255,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="39" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="130,150,240,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="transport/transport_parking.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="4" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,160,100,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="amenity/amenity_library.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="40" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,100,255,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="41" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,100,255,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="1" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="food/food_bar.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="255,255,255,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="42" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,100,255,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="1" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="food/food_pub.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="255,255,255,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="43" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,100,255,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="1" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="food/food_restaurant.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="255,255,255,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="44" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,120,200,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="45" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,120,200,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="1" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="gpsicons/bank.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="255,255,255,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="46" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,120,200,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="shopping/shopping_convenience.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="255,255,255,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="47" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,120,200,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="services/tourism=hotel.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="48" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="245,245,245,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,1.19999999999999996"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="78,78,78,255"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.1"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="243,243,243,0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="half_square"/>
              <Option type="QString" name="offset" value="0.90000000000000002,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="91,84,84,255"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.1"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="244,244,244,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,-1.19999999999999996"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="78,78,78,255"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.1"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="245,245,245,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="half_square"/>
              <Option type="QString" name="offset" value="0.90000000000000002,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="78,78,78,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="60,180,60,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,1.19999999999999996"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,200,0,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,-0.00000000000000006"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="255,0,0,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,-1.19999999999999996"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="5" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,160,100,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="tourist/tourist_museum.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="6" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,160,100,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="amenity/amenity_police.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="7" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="200,160,100,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="amenity/amenity_post_box.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="3.6"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="8" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="1" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="80,180,60,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,0"/>
              <Option type="QString" name="outline_style" value="no"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="1"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol clip_to_extent="1" frame_rate="10" type="marker" name="9" force_rhr="0" alpha="1" is_animated="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </data_defined_properties>
          <layer locked="0" pass="2" class="SimpleMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="cap_style" value="square"/>
              <Option type="QString" name="color" value="80,180,60,255"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="joinstyle" value="bevel"/>
              <Option type="QString" name="name" value="circle"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="26,115,232,0"/>
              <Option type="QString" name="outline_style" value="solid"/>
              <Option type="QString" name="outline_width" value="0.3"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="4"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer locked="0" pass="0" class="SvgMarker" enabled="1">
            <Option type="Map">
              <Option type="QString" name="angle" value="0"/>
              <Option type="QString" name="color" value="255,255,255,255"/>
              <Option type="QString" name="fixedAspectRatio" value="0"/>
              <Option type="QString" name="horizontal_anchor_point" value="1"/>
              <Option type="QString" name="name" value="shopping/shopping_photo.svg"/>
              <Option type="QString" name="offset" value="0,0"/>
              <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offset_unit" value="MM"/>
              <Option type="QString" name="outline_color" value="35,35,35,255"/>
              <Option type="QString" name="outline_width" value="0"/>
              <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="outline_width_unit" value="MM"/>
              <Option name="parameters"/>
              <Option type="QString" name="scale_method" value="diameter"/>
              <Option type="QString" name="size" value="2.75"/>
              <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="size_unit" value="MM"/>
              <Option type="QString" name="vertical_anchor_point" value="1"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
      </symbols>
    </renderer-v2>
    <symbol clip_to_extent="1" frame_rate="10" type="marker" name="centerSymbol" force_rhr="0" alpha="1" is_animated="0">
      <data_defined_properties>
        <Option type="Map">
          <Option type="QString" name="name" value=""/>
          <Option name="properties"/>
          <Option type="QString" name="type" value="collection"/>
        </Option>
      </data_defined_properties>
      <layer locked="0" pass="0" class="SimpleMarker" enabled="0">
        <Option type="Map">
          <Option type="QString" name="angle" value="0"/>
          <Option type="QString" name="cap_style" value="square"/>
          <Option type="QString" name="color" value="245,75,80,0"/>
          <Option type="QString" name="horizontal_anchor_point" value="1"/>
          <Option type="QString" name="joinstyle" value="bevel"/>
          <Option type="QString" name="name" value="circle"/>
          <Option type="QString" name="offset" value="0,0"/>
          <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
          <Option type="QString" name="offset_unit" value="MM"/>
          <Option type="QString" name="outline_color" value="35,35,35,0"/>
          <Option type="QString" name="outline_style" value="solid"/>
          <Option type="QString" name="outline_width" value="0"/>
          <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
          <Option type="QString" name="outline_width_unit" value="MM"/>
          <Option type="QString" name="scale_method" value="diameter"/>
          <Option type="QString" name="size" value="1"/>
          <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
          <Option type="QString" name="size_unit" value="MM"/>
          <Option type="QString" name="vertical_anchor_point" value="1"/>
        </Option>
        <data_defined_properties>
          <Option type="Map">
            <Option type="QString" name="name" value=""/>
            <Option name="properties"/>
            <Option type="QString" name="type" value="collection"/>
          </Option>
        </data_defined_properties>
      </layer>
    </symbol>
  </renderer-v2>
  <labeling type="rule-based">
    <rules key="{2978be35-8b03-44fa-9e14-e1b9794665dc}">
      <rule description="Park" key="{323740c7-aeb8-41c0-9cf5-9e6fa08b3172}" scalemaxdenom="7500" scalemindenom="1" filter="&quot;Category&quot; ~ 'Attraction|MortuaryCemetery|Park(?!ing)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="9" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="80,180,60,255" fontStrikeout="0" legendString="Pk">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="1" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="3" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="0"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if(@id % 2 = 0, 5, 3)"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule description="Emergency" key="{59bf907f-77c0-47fd-82bc-08a7796f43a4}" scalemaxdenom="20000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Fire|Health'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="9" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="239,144,144,255" fontStrikeout="0" legendString="Em">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="1" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="3" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="0"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if(@id % 2 = 0, 5, 3)"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule description="Government" key="{34cb997b-3f61-45fb-806a-de35b3a63744}" scalemaxdenom="15000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Admin|Education|Police|Post(?!Box)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="9" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="162,137,88,255" fontStrikeout="0" legendString="Gv">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="0" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="3" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="0"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="AutoWrapLength">
                  <Option type="bool" name="active" value="false"/>
                  <Option type="int" name="type" value="1"/>
                  <Option type="QString" name="val" value=""/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if(@id % 2 = 0, 5, 3)"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule description="Airport" key="{c491202c-6b9b-46eb-b0ba-52852edb225d}" scalemaxdenom="50000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Building(Cargo|Passenger)Airplane'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="8" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="20,145,230,255" fontStrikeout="0" legendString="Tr">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="0" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="2.5" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="0"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="AutoWrapLength">
                  <Option type="bool" name="active" value="false"/>
                  <Option type="int" name="type" value="1"/>
                  <Option type="QString" name="val" value=""/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if(@id % 2 = 0, 5, 3)"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule description="Bus / Tram Stops" key="{72858d75-0da2-4956-894c-58c970a814a4}" scalemaxdenom="2500" scalemindenom="1" filter="&quot;Category&quot; ~ 'Stop(?=Bus|Tram)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="8" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="20,145,230,255" fontStrikeout="0" legendString="Tr">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="0" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="2.5" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="0"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="AutoWrapLength">
                  <Option type="bool" name="active" value="false"/>
                  <Option type="int" name="type" value="1"/>
                  <Option type="QString" name="val" value=""/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if(@id % 2 = 0, 5, 3)"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule description="Transportation" key="{f3e9f96a-1279-4b9e-b9c4-560e56bb9828}" scalemaxdenom="30000" scalemindenom="1" filter="&quot;Category&quot; ~ 'Building(Bus|PassengerShip|PassengerTrain|Subway|Tram)' AND NOT &quot;Category&quot; ~ 'Airplane'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="9" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="20,145,230,255" fontStrikeout="0" legendString="Tr">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="1" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="3" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="-3"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="LabelDistance">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if ((&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'), 5, if ((&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'), 7, 3))"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="5"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule description="Stores" key="{6e609f46-122d-4755-b73a-61a1ec94459f}" scalemaxdenom="4000" scalemindenom="1" filter="&quot;Category&quot; ~ '(?:Store(?:Bank|Bar|Beverage|ConvenienceStore|Food|GasStation|Hotel|Restaurant)|Parking)'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="9" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="0,0,0,255" fontStrikeout="0" legendString="St">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="1" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="3" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="0"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="Color">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if(&quot;Category&quot; ~ 'Store(Bank|ConvenienceStore|Hotel)', '#ff78c8', if(&quot;Category&quot; ~ 'Store(Bar|Beverage|Food|Restaurant)', '#c864ff', if(&quot;Category&quot; ~ 'StoreGasStation|Parking', '#8296f0', '#ff78c8')))"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="5"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule description="Utility" key="{dfee978f-c7a4-4309-b17e-bb9c7324fdec}" scalemaxdenom="7500" scalemindenom="1" filter="&quot;Category&quot; ~ 'Power|Sewage|Waste|Water'">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="9" multilineHeight="1" allowHtml="0" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" textOrientation="horizontal" fontWordSpacing="0" textOpacity="1" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="50,150,150,255" fontStrikeout="0" legendString="Ut">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="1"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="1" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="0" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="5" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="7" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="3" allowDegraded="0" distUnits="MM" placement="6" overrunDistance="0" xOffset="3" yOffset="-3"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option type="Map" name="properties">
                <Option type="Map" name="LabelDistance">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="if ((&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'), 5, if ((&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'), 7, 3))"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="5"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" name="active" value="true"/>
                  <Option type="QString" name="expression" value="'R,L'"/>
                  <Option type="int" name="type" value="3"/>
                </Option>
              </Option>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{db997be3-9234-42c9-8c43-189eefc59ebf}" filter="ELSE">
        <settings calloutType="simple">
          <text-style fontFamily="Segoe UI" isExpression="1" fontLetterSpacing="0" fontItalic="0" fontUnderline="0" previewBkgrdColor="255,255,255,255" namedStyle="Semibold" fontSizeUnit="Point" useSubstitutions="0" multilineHeightUnit="Percentage" capitalization="0" fontSize="1" multilineHeight="1" allowHtml="0" fieldName="0" textOrientation="horizontal" fontWordSpacing="0" textOpacity="0" fontKerning="1" fontWeight="63" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" forcedBold="0" blendMode="0" textColor="255,255,255,255" fontStrikeout="0" legendString="Aa">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM" bufferNoFill="1" bufferJoinStyle="128" bufferBlendMode="0" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferDraw="0"/>
            <text-mask maskSize="0" maskedSymbolLayers="" maskSizeUnits="MM" maskJoinStyle="128" maskType="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskEnabled="0" maskOpacity="1"/>
            <background shapeSVGFile="" shapeBorderWidth="0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetY="0" shapeBorderColor="128,128,128,255" shapeRadiiUnit="Point" shapeType="0" shapeSizeUnit="Point" shapeJoinStyle="64" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeSizeType="0" shapeFillColor="255,255,255,255" shapeBorderWidthUnit="Point" shapeBlendMode="0" shapeRadiiX="0" shapeRotationType="0" shapeOpacity="1" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeSizeY="0" shapeRadiiY="0" shapeDraw="0" shapeOffsetX="0" shapeSizeX="0">
              <symbol clip_to_extent="1" frame_rate="10" type="marker" name="markerSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleMarker" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="angle" value="0"/>
                    <Option type="QString" name="cap_style" value="square"/>
                    <Option type="QString" name="color" value="152,125,183,255"/>
                    <Option type="QString" name="horizontal_anchor_point" value="1"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="name" value="circle"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="35,35,35,255"/>
                    <Option type="QString" name="outline_style" value="solid"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="outline_width_unit" value="MM"/>
                    <Option type="QString" name="scale_method" value="diameter"/>
                    <Option type="QString" name="size" value="2"/>
                    <Option type="QString" name="size_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="size_unit" value="MM"/>
                    <Option type="QString" name="vertical_anchor_point" value="1"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol clip_to_extent="1" frame_rate="10" type="fill" name="fillSymbol" force_rhr="0" alpha="1" is_animated="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" name="name" value=""/>
                    <Option name="properties"/>
                    <Option type="QString" name="type" value="collection"/>
                  </Option>
                </data_defined_properties>
                <layer locked="0" pass="0" class="SimpleFill" enabled="1">
                  <Option type="Map">
                    <Option type="QString" name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="color" value="255,255,255,255"/>
                    <Option type="QString" name="joinstyle" value="bevel"/>
                    <Option type="QString" name="offset" value="0,0"/>
                    <Option type="QString" name="offset_map_unit_scale" value="3x:0,0,0,0,0,0"/>
                    <Option type="QString" name="offset_unit" value="MM"/>
                    <Option type="QString" name="outline_color" value="128,128,128,255"/>
                    <Option type="QString" name="outline_style" value="no"/>
                    <Option type="QString" name="outline_width" value="0"/>
                    <Option type="QString" name="outline_width_unit" value="Point"/>
                    <Option type="QString" name="style" value="solid"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" name="name" value=""/>
                      <Option name="properties"/>
                      <Option type="QString" name="type" value="collection"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowBlendMode="6" shadowOffsetDist="1" shadowRadiusUnit="MM" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowRadiusAlphaOnly="0" shadowScale="100" shadowOffsetAngle="135" shadowOffsetUnit="MM" shadowRadius="1.5" shadowOpacity="0.69999999999999996" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowDraw="0" shadowUnder="0" shadowColor="0,0,0,255"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" reverseDirectionSymbol="0" decimals="3" plussign="0" wrapChar="" multilineAlign="3" autoWrapLength="12" useMaxLineLengthForAutoWrap="1" addDirectionSymbol="0" leftDirectionSymbol="&lt;" formatNumbers="0" placeDirectionSymbol="0"/>
          <placement rotationUnit="AngleDegrees" centroidInside="0" fitInPolygonOnly="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" distMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" offsetType="1" lineAnchorTextPoint="FollowPlacement" overlapHandling="PreventOverlap" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" offsetUnits="MM" centroidWhole="0" maxCurvedCharAngleIn="25" quadOffset="4" lineAnchorType="0" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" priority="5" maxCurvedCharAngleOut="-25" repeatDistance="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" repeatDistanceUnits="MM" geometryGeneratorType="PointGeometry" lineAnchorClipping="0" polygonPlacementFlags="2" placementFlags="10" rotationAngle="0" preserveRotation="1" lineAnchorPercent="0.5" geometryGeneratorEnabled="0" layerType="PointGeometry" dist="0" allowDegraded="0" distUnits="MM" placement="1" overrunDistance="0" xOffset="0" yOffset="0"/>
          <rendering upsidedownLabels="0" obstacle="1" maxNumLabels="2000" obstacleFactor="1.6000000000000001" fontLimitPixelSize="0" drawLabels="1" scaleMax="0" fontMinPixelSize="3" mergeLines="0" unplacedVisibility="0" scaleVisibility="0" limitNumLabels="0" minFeatureSize="0" obstacleType="1" labelPerPart="0" fontMaxPixelSize="10000" scaleMin="0" zIndex="0"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" name="name" value=""/>
              <Option name="properties"/>
              <Option type="QString" name="type" value="collection"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" name="anchorPoint" value="pole_of_inaccessibility"/>
              <Option type="int" name="blendMode" value="0"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" name="name" value=""/>
                <Option name="properties"/>
                <Option type="QString" name="type" value="collection"/>
              </Option>
              <Option type="bool" name="drawToAllParts" value="false"/>
              <Option type="QString" name="enabled" value="0"/>
              <Option type="QString" name="labelAnchorPoint" value="point_on_exterior"/>
              <Option type="QString" name="lineSymbol" value="&lt;symbol clip_to_extent=&quot;1&quot; frame_rate=&quot;10&quot; type=&quot;line&quot; name=&quot;symbol&quot; force_rhr=&quot;0&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer locked=&quot;0&quot; pass=&quot;0&quot; class=&quot;SimpleLine&quot; enabled=&quot;1&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;align_dash_pattern&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;capstyle&quot; value=&quot;square&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash&quot; value=&quot;5;2&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;customdash_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;draw_inside_polygon&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;joinstyle&quot; value=&quot;bevel&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_color&quot; value=&quot;60,60,60,255&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_style&quot; value=&quot;solid&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width&quot; value=&quot;0.3&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;line_width_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;offset_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;ring_filter&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;use_custom_dash&quot; value=&quot;0&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; name=&quot;name&quot; value=&quot;&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; name=&quot;type&quot; value=&quot;collection&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>"/>
              <Option type="double" name="minLength" value="0"/>
              <Option type="QString" name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="minLengthUnit" value="MM"/>
              <Option type="double" name="offsetFromAnchor" value="0"/>
              <Option type="QString" name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromAnchorUnit" value="MM"/>
              <Option type="double" name="offsetFromLabel" value="0"/>
              <Option type="QString" name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0"/>
              <Option type="QString" name="offsetFromLabelUnit" value="MM"/>
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
