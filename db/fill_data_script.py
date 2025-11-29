import psycopg2
from faker import Faker
from random import choice, randint, sample
import os

# Инициализация Faker с локализацией (например, ru_RU для русских названий)
fake = Faker('ru_RU')

# Параметры подключения к базе данных (замените на свои)
db_params = {
    'dbname': os.getenv("DB_NAME"),
    'user': os.getenv("DB_USER"),
    'password': os.getenv("DB_PASSWORD"),
    'host': os.getenv("DB_HOST"),
    'port': os.getenv("DB_PORT")
}

# Данные для заполнения
resorts = ['Сейшелы', 'Египет', 'Турция', 'ОАЭ', 'Тайланд', 'Куба', 'Мальдивы', 'Доминикана']
categories = ['5 звезд', '4 звезды', '3 звезды', 'Apart', 'Special', 'Guest-house', 'Boutique']
food_types = ['Все включено', 'Полный пансион', 'Полупансион', 'Завтрак', 'Без питания', 'Особые типы питания']
concepts = ['Рекомендуемые отели', 'SPA', 'Семейный отдых', 'Экономичный', 'Элит сервис', 'Отели делюкс']
beach_types = ['Галечный пляж', 'Песчаный пляж', 'Песчано-галечный пляж', 'Каменистый пляж']
room_types = ['Стандартный', 'Комната для семьи', 'Просторный люкс', 'Вилла']
services = ['Wi-Fi', 'Дискотека', 'СПА', 'Массаж', 'Хамам', 'Сауна', 'Доктор', 'Сейф', 'Казино']

# Случайные расстояния до пляжа (в метрах)
beach_distances = [50, 250, 500]

# Изображения для номеров
room_photos = [
    'https://content.coral.ru/resize/576x522/media/image/31/63734/638749653691094413.jpg',
    'https://content.coral.ru/resize/576x522/media/image/31/63734/638749657995187003.jpg',
    'https://content.coral.ru/resize/576x522/media/image/31/63734/638948508495190629.jpg'
]

# Изображения для отелей по курортам (пример, замените полным словарем)
hotel_photos = {
    'Сейшелы': [
        'https://attaches.1001tur.ru/hotels/gallery/56540/60111597205456.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/5910/17525612044198.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/668767/9221675946941.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/667144/73501600874425.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/438725/75441696337173.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/279563/4991696398527.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/427141/17060940936104.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/278087/73331750060854.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/5909/169634104684.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/427149/81101689366699.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/396703/67551621503948.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/714372/89321637608698.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/445906/371621500173.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/278511/86651621499121.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/732369/70761732614422.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/633286/30891703195821.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/555651/101746061880.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/555657/45231626781542.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/57623/61611597245533.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/4540/72211675865192.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/714384/36491687039655.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/56538/99001621504536.jpg'
    ],
    'Египет': [
        'https://attaches.1001tur.ru/hotels/gallery/639484/58471596431894.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/609253/2561567518913.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/716265/3011621438025.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/637276/41761694627211.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/725181/60211613489462.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/278171/57691594997748.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/551928/3221665571906.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/633538/13881596129097.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/562929/17322665936129.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/728443/79881740036941.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/51927/35681620925205.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/726525/72721670752685.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/642493/10851596593202.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/341860/4621671625975.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/732324/21241732541370.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/276801/38191667996499.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3799/17512727643343.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/57447/71091620925593.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/42/48141654282951.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/623560/17615494772258.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/623512/16830294574763.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/739647/1740039988560.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/551774/16786925538212.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/612060/16965834041156.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/443009/37261740038136.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/718422/16732567504654.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604855/42691733129174.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/54442/49501640165916.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/419530/13961594997848.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/56866/16758369602385.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/731253/39461716446126.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/279534/16071740036441.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/744963/17436738978070.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/696999/20111684537887.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/696057/17116975255927.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/594976/16748131784811.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604414/66761628074115.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/57378/45101506584490.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/727803/25241670940173.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2759/003.jpeg',
        'https://attaches.1001tur.ru/hotels/gallery/55042/17151628472086.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2131/2061506587769.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/143/95891652791716.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/743382/17400412576618.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/698553/82211621344113.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/601928/17623259943048.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/693468/56001664287396.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/551748/17252645301281.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/59320/57071660651580.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/751154/31211759140644.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/607354/59371677497643.jpg'
    ],
    'Турция': [
        'https://attaches.1001tur.ru/hotels/gallery/452/17395188427228.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/434087/24141686611024.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/569684/85071627993969.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/622237/83021621522162.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/462556/16766270003383.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/430231/16814620376433.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449559/50091627995943.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/430229/17434973856804.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/1448/46901621319567.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/468971/1521684639024.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/614121/15774540876817.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/445636/26821498165190.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/445558/45131694546216.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/609639/16890759502489.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/473261/34851621425009.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/429723/16838984321334.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/6485/79121679993535.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/320922/18971621190801.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/579501/40111501892001.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/445904/38231642457836.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/470886/94021647209858.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/430287/53751595040895.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/320543/87921621380829.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/629434/59181633741435.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/465684/69991621235613.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/434500/17543901433012.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/558118/40871504645290.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/451331/13221653661610.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/750240/62441754401643.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/691437/21321636960472.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/429515/95911654374307.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/712221/8391691657338.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/622084/61431594928072.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/470794/17020263134030.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/320583/1756983303316.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/557818/21551600950488.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/465170/85611621224555.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/49848/15231628225379.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/322360/17465230164742.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/745617/31041746616544.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/15053/75311655666593.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/465895/31181498198580.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/622111/55231594928428.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/599572/67121498158861.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/577715/5511498156427.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/321692/87001621377957.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/414908/39521621212071.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/631225/56221621533015.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/429410/59421673333782.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/465525/60941636134650.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/278983/78191658925853.jpg'
    ],
    'ОАЭ': [
        'https://attaches.1001tur.ru/hotels/gallery/50426/15773675677238.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/293349/16765317571585.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/621322/26671630747093.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/408970/1561405516193.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/447905/29391630155522.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/513753/13121620327158.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/320466/821427663381ae6d295f678f44f69839df055a89b257.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/688755/31001609703842.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/647755/52941597438142.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554910/13941674431172.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/5662/27151620328896.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/53584/11431660538197.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/513464/98441695054879.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449858/90221600918977.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/576254/85371698314291.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/728455/12531629608985.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/58232/61791642170587.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/601463/47651664211494.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/391032/2081620308944.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/621304/88871594871972.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/599488/481668466822.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/720492/11611646030557.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/52654/51491718706786.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/4899/39181620295850.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/513457/96631620309526.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3555/15754668621011.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/448703/99261620330854.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/730764/30901711374854.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/603655/29531541596911.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554996/2561498141935.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/341470/22231659656480.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554947/23581600940232.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604702/92101541693686.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/53959/17.jpeg',
        'https://attaches.1001tur.ru/hotels/gallery/513481/17981498155434.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2135/86181620290333.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/513363/32761620292756.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/392365/40461620297779.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604159/46431683705309.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/562235/5781600960316.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/513347/17454877501958.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/52617/71501644419343.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/425036/8181595025457.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/278890/55481620307733.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/605692/82431742596527.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/652039/35031734375497.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/717597/21051620328623.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/568400/64451723518495.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/724869/25991620305418.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604699/7431541693651.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/732435/44651732623988.jpg'
    ],
    'Тайланд': [
        'https://attaches.1001tur.ru/hotels/gallery/334379/74351701053354.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/279283/40511665143246.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/557261/35511673485385.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604801/25051537459150.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/732672/88071732687941.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/602873/25651661683066.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/732678/17326899053091.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/602879/87571626936137.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449839/23831505127906.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/718899/58131641471577.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/728563/56591681457531.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729353/16944389272728.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/428922/15732187265884.png',
        'https://attaches.1001tur.ru/hotels/gallery/566545/97741504711147.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/730128/88781760529786.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/568252/21971669382038.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/485656/58771543903726.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/647038/47561639099991.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2618/17537709719787.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/600250/90851663543962.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/58529/95011735213523.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/336217/80991693243603.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/602870/99721537540099.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/625486/71301595037915.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/614880/5731572563700.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729558/34651694504792.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/662023/72811599167046.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/633355/4701630785078.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449520/64341627139638.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/736413/8931737540161.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/630772/39521596051726.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/601013/52311667665057.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604957/52781706595493.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/279017/74101732710310.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/443720/12801595034903.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/653407/17250108428281.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/276896/57461667481255.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/573643/56961600987153.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/602852/9961537459331.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/342616/71781688697739.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/707409/10091663694724.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/731709/51061731407718.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/428590/88761627021070.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/730806/78511711460832.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/633226/19401756966641.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/732840/50631732709300.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/567016/10441668078097.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/557368/69251626836741.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/630961/78671596056895.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604804/94511630737619.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/630664/34731630787211.jpg'
    ],
    'Куба': [
        'https://attaches.1001tur.ru/hotels/gallery/441742/1673449456661.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/730398/17062737019085.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/54611/87261505885380.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449779/1693554799772.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449445/1911470906851.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/693198/16964138296082.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/603358/15409824995499.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/4754/37501506433993.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3860/50901596047183.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449802/1714113319439.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/731919/1729579129532.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/727680/16734422581055.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/731814/17274428023845.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/728158/16756768291662.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/54684/15738047826169.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3858/17434137334408.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/730392/17062707464633.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/54683/1573741232335.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/633568/96621581766165.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3865/007.jpeg',
        'https://attaches.1001tur.ru/hotels/gallery/725313/17129081878710.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/668602/70721600879961.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/448530/16733535188905.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/54606/1.jpeg',
        'https://attaches.1001tur.ru/hotels/gallery/4312/20561506433370.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/610785/15738246323539.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/423979/39771616337103.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/671725/16733581157971.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3886/17242277915905.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3868/17302730902169.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/562275/84191504676415.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/726663/52531670752693.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554422/16738688495154.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/438811/85301505723778.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/749934/17531692823229.jpeg',
        'https://attaches.1001tur.ru/hotels/gallery/697296/95571621278532.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/4794/70911506432883.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/4310/1734427293353.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3862/17135099001530.jpeg',
        'https://attaches.1001tur.ru/hotels/gallery/669484/17062726998553.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3882/10451506432587.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3864/1.jpeg',
        'https://attaches.1001tur.ru/hotels/gallery/671053/1711543094680.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/730395/17062719457230.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3869/17162758075962.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/673042/75741600895517.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/588139/9371505462704.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/746301/17489567572108.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/279463/17538742017591.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/57343/1701935483333.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/610824/16968402562442.jpg'
    ],
    'Мальдивы': [
        'https://attaches.1001tur.ru/hotels/gallery/611442/51811670947497.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554588/96641683142132.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/3611/94311565117350.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/730134/61011703704797.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/1827/67161537455711.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/57927/86921675849279.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/435350/92521679040073.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/435352/39371663673894.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/449959/28101665488819.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729459/16933802177128.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/744612/17429040638508.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/605944/36111661672157.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/613449/49091665999093.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729513/36881694439562.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/633685/17511596061426.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/394471/6721664017377.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554580/68811668416339.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729498/81911694374113.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/1830/17258806162566.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/609735/89141571318249.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/1818/74951678873779.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/631672/19541596077452.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/605950/30271630729153.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/602666/37421537455512.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/605947/1700550859105.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/605941/71221626543892.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/424343/16934884556526.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/631996/33461596085440.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/611427/3321565117974.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554570/54611654256077.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/424336/95401571394889.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/609996/13581537455578.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/424316/81171663769945.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/731220/19821715850038.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729489/84871694439550.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/451603/9431542175297.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/54123/69381749831856.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/554572/73971693489193.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/728314/31091702142765.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/339643/951671452457.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/744657/71161742977955.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/714465/1726485065941.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/652177/16832892627162.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/561541/95051678873851.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/744609/17429019725259.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729180/96031690293573.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/442121/61081663837332.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/565714/55381626546407.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/563972/24651688596952.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/610074/98251630731092.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/728311/80761678966780.jpg'
    ],
    'Доминикана': [
        'https://attaches.1001tur.ru/hotels/gallery/6293/571482768643.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/419369/52961623442864.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2425/15833264627003.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/716487/45361630887135.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604609/73481541756209.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/5721/84061630569102.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/5712/69111678190168.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/435561/1571485271593.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2405/64711678188520.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/729822/55471697960833.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/551701/95311504597216.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/691902/50211613479756.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/440426/421483021603.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/603224/2721630736450.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/600130/33201524759860.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/551682/96551732864818.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/633784/71881596069494.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/438662/1771484063658.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/567664/72131732776622.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/663766/87361600093728.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/419388/17537052261116.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/446901/13981679338246.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/551679/231486044977.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/595030/80981601023236.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/603035/55631544422882.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604993/15432224864275.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/603032/29271660136106.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/407851/1951484062620.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/604429/88851541750137.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/600172/75791524760834.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/561719/51701504671859.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/599521/15834966047996.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/606256/96611666301948.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/607348/9441564726693.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/739899/42451733490650.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/609610/15706212373534.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/58013/5291732864800.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/54186/89141506591485.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/599482/60051524737275.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/597252/64871506935397.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/674071/74681686279026.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/551690/98081732776631.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2409/25281506591224.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/450284/99341524736225.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/605587/7191547722102.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/631501/3201596072618.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/595036/34951506591048.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2403/23571506592012.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/631669/3431596077709.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/2433/43761623443105.jpg',
        'https://attaches.1001tur.ru/hotels/gallery/631804/52371660317147.jpg'
    ]
}

# Примерные локации для каждого курорта
locations = {
    'Сейшелы': ['Маэ', 'Праслин', 'Ла-Диг', 'Силуэт'],
    'Египет': ['Хургада', 'Шарм-эль-Шейх', 'Марса-Алам', 'Дахаб'],
    'Турция': ['Анталья', 'Кемер', 'Аланья', 'Бодрум', 'Мармарис'],
    'ОАЭ': ['Дубай', 'Абу-Даби', 'Шарджа', 'Фуджейра'],
    'Тайланд': ['Пхукет', 'Паттайя', 'Краби', 'Самуи'],
    'Куба': ['Варадеро', 'Гавана', 'Кайо-Коко', 'Кайо-Ларго'],
    'Мальдивы': ['Мале', 'Ари Атолл', 'Баа Атолл', 'Раа Атолл'],
    'Доминикана': ['Пунта-Кана', 'Баваро', 'Ла-Романа', 'Пуэрто-Плата']
}

# Функция для генерации цены номера
def get_price_for_category(category):
    price_ranges = {
        '5 звезд': (50000, 120000),
        '4 звезды': (30000, 80000),
        '3 звезды': (10000, 40000),
        'Apart': (15000, 50000),
        'Special': (20000, 60000),
        'Guest-house': (5000, 25000),
        'Boutique': (35000, 100000)
    }
    min_price, max_price = price_ranges.get(category, (5000, 120000))
    return randint(min_price, max_price)

# Функция для генерации названия отеля с помощью Faker
def generate_hotel_name(resort, category, photo_index):
    prefix = fake.word().capitalize()
    suffix = choice(['Отель', 'Резорт', 'Вилла', 'Палас'])
    return f"{prefix} {suffix} {category} #{photo_index} ({resort})"

# Функция для генерации уникального описания отеля с помощью Faker
def generate_hotel_description(resort, location, category, beach_type, beach_distance, food_type, concept, services, photo_index):
    service_list = ", ".join(sample(services, k=min(4, len(services))) if services else [])
    description = fake.paragraph(nb_sentences=5, variable_nb_sentences=True)
    custom_info = (
        f"Расположен в {location}, {resort}, в {beach_distance} метрах от {beach_type.lower()}. "
        f"Категория: {category}. Идеально для {concept.lower()}. "
        f"Питание: {food_type.lower()}. Услуги: {service_list or 'без дополнительных услуг'}. "
        f"Отель #{photo_index} на {resort}."
    )
    return f"{description} {custom_info}"

# Функция для генерации уникального описания номера с помощью Faker
def generate_room_description(room_type, beach_type, services, room_index):
    service_list = ", ".join(sample(services, k=min(2, len(services))) if services else [])
    description = fake.paragraph(nb_sentences=3, variable_nb_sentences=True)
    custom_info = (
        f"{room_type} с видом на {beach_type.lower()}. "
        f"Оснащение: {service_list or 'без дополнительных услуг'}. "
        f"Номер #{room_index}."
    )
    return f"{description} {custom_info}"

try:
    # Подключение к базе данных
    conn = psycopg2.connect(**db_params)
    cursor = conn.cursor()

    # Вставка трёх администраторов с разными уровнями доступа
    access_levels = ['viewer', 'editor', 'superadmin']
    admin_ids = []
    for access_level in access_levels:
        cursor.execute("""
                       INSERT INTO admin (access_level, created_by_id)
                       VALUES (%s, NULL)
                           RETURNING admin_id
                       """, (access_level,))
        admin_id = cursor.fetchone()[0]
        admin_ids.append(admin_id)

    # Вставка отелей и связанных данных
    for resort in resorts:
        photo_index = 1  # Счётчик для нумерации отелей
        for photo in hotel_photos[resort]:
            # Генерация данных отеля
            category = choice(categories)
            location = choice(locations[resort])
            name = generate_hotel_name(resort, category, photo_index)
            beach_type = choice(beach_types)
            beach_distance = choice(beach_distances)
            food_type = choice(food_types)
            concept = choice(concepts)
            room_type = choice(room_types)

            # Случайный выбор сервисов (от 0 до всех)
            selected_services = sample(services, k=randint(0, len(services)))

            description = generate_hotel_description(
                resort, location, category, beach_type, beach_distance,
                food_type, concept, selected_services, photo_index
            )
            minimal_price_per_night = 0  # Будет обновлено триггером

            # Вставка отеля
            cursor.execute("""
                           INSERT INTO hotel (resort, location, name, category, description, food_type,
                                              beach_distance, beach_type, room_type, conception, minimal_price_per_night)
                           VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
                               RETURNING hotel_id
                           """, (resort, location, name, category, description, food_type,
                                 beach_distance, beach_type, room_type, concept, minimal_price_per_night))
            hotel_id = cursor.fetchone()[0]

            # Вставка фотографии отеля
            cursor.execute("""
                           INSERT INTO hotel_photo (file_path, hotel_id)
                           VALUES (%s, %s)
                           """, (photo, hotel_id))

            # Вставка сервисов отеля
            for service in selected_services:
                cursor.execute("""
                               INSERT INTO hotel_service (name, hotel_id)
                               VALUES (%s, %s)
                               """, (service, hotel_id))

            # Вставка трёх номеров для отеля
            for i in range(3):
                room_type = choice(room_types)
                price_per_night = get_price_for_category(category)
                room_description = generate_room_description(room_type, beach_type, selected_services, i + 1)

                # Вставка номера
                cursor.execute("""
                               INSERT INTO room (hotel_id, price_per_night, description)
                               VALUES (%s, %s, %s)
                                   RETURNING room_id
                               """, (hotel_id, price_per_night, room_description))
                room_id = cursor.fetchone()[0]

                # Вставка фотографии номера
                cursor.execute("""
                               INSERT INTO room_photo (file_path, room_id)
                               VALUES (%s, %s)
                               """, (room_photos[i], room_id))

            photo_index += 1

    # Подтверждение транзакции
    conn.commit()
    print(f"Данные успешно вставлены в базу данных. Создано {sum(len(photos) for photos in hotel_photos.values())} отелей, "
          f"{sum(len(photos) for photos in hotel_photos.values()) * 3} номеров, 3 администратора и сервисы для отелей.")

except Exception as e:
    print(f"Ошибка: {e}")
    conn.rollback()

finally:
    cursor.close()
    conn.close()
