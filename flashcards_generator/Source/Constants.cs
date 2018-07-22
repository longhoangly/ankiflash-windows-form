using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FlashcardsGenerator.Source
{
    public struct OpenFileDialog
    {
        public const string TITLE = @"Browse Text Files";
        public const string INITIAL_PATH = @"C:\";
        public const string DEFAULT_EXT = @"txt";
        public const string FILTER = @"Text files (*.txt)|*.txt|All files (*.*)|*.*";
    }

    public struct Button
    {
        public const string RUN = @"Run";
        public const string RUNNING = @"Running...";
    }

    public struct MessageBoxProps
    {
        public const string INFO = @"Info";
        public const string FAILED = @"Failed";
        public const string ERROR = @"Error";

        public const string CANCELED = @"Canceled!";
        public const string COMPLETED = @"Completed";
        public const string ERROR_OCCUR = @">>> The following error occurs <<<";
        public const string CANNOT_CONNECT_TO_DICTIONARY = @"Cannot connect to the dictionary.\nPlease check your connection!";
    }

    public struct GeneratingStatus
    {
        public const string CONNECTION_FAILED = "<CONNECTION_FAILED>";
        public const string SPELLING_WRONG = "<SPELLING_WRONG>";
        public const string GENERATING_SUCCEEDED = "<GENERATING_SUCCEEDED>";
        public const string GENERATING_FAILED = "<GENERATING_FAILED>";
    }

    public struct FileName
    {
        public const string ANKI_DECK = "ankiDeck.csv";
        public const string LANGUAGE = "Language.txt";

        // OXFORD
        public const string INTERFACE_CSS = "interface.css";
        public const string OXFORD_CSS = "oxford.css";
        public const string RESPONSIVE_CSS = "responsive.css";
        public const string BTN_WORDLIST_PNG = "btn-wordlist.png";
        public const string USONLY_AUDIO_PNG = "usonly-audio.png";
        public const string ENLARGE_IMG_PNG = "enlarge-img.png";
        public const string ENTRY_ARROW_PNG = "entry-arrow.png";
        public const string ENTRY_BULLET_PNG = "entry-bullet.png";
        public const string ENTRY_SQBULLET_PNG = "entry-sqbullet.png";
        public const string GO_TO_TOP_PNG = "go-to-top.png";
        public const string ICON_ACADEMIC_PNG = "icon-academic.png";
        public const string ICON_AUDIO_BRE_PNG = "icon-audio-bre.png";
        public const string ICON_AUDIO_NAME_PNG = "icon-audio-name.png";
        public const string ICON_OX3000_PNG = "icon-ox3000.png";
        public const string ICON_PLUS_MINUS_PNG = "icon-plus-minus.png";
        public const string ICON_PLUS_MINUS_GREY_PNG = "icon-plus-minus-grey.png";
        public const string ICON_PLUS_MINUS_ORANGE_PNG = "icon-plus-minus-orange.png";
        public const string ICON_SELECT_ARROW_CIRRLE_BLUE_PNG = "icon-select-arrow-circle-blue.png";
        public const string LOGIN_BG_PNG = "login-bg.png";
        public const string PVARR_PNG = "pvarr.png";
        public const string PVARR_BLUE_PNG = "pvarr-blue.png";
        public const string SEARCH_MAG_PNG = "search-mag.png";

        // SOHA
        public const string MAIN_MIN_CSS = "main_min.css";
        public const string DOT_JPG = "dot.jpg";
        public const string MINUS_SECTION_JPG = "minus_section.jpg";
        public const string PLUS_SECTION_JPG = "plus_section.jpg";
        public const string HIDDEN_JPG = "hidden.jpg";
        public const string EXTERNAL_PNG = "external.png";

        // COLLINS
        public const string COLLINS_CSS = "collins_common.css";
        public const string ICONS_RIGHT_PNG = "icons-right.png";

        // LACVIET
        public const string HOME_CSS = "home.css";
        public const string ICON_6_7_PNG = "Icon_6_7.png";
        public const string ICON_7_4_PNG = "Icon_7_4.png";

        // CAMBRIDGE
        public const string COMMON_CSS = "common.css";
        public const string STAR_PNG = "star.png";

        public const string EN_SINGLE_FORM_ABCDEFGHLONGLEE123 = "[EN]singleformABCDEFGHLONGLEE123.apkg";
        public const string EN_MULTIPLE_FORM_ABCDEFGHLONGLEE123 = "[EN]multiformABCDEFGHLONGLEE123.apkg";
        public const string FR_SINGLE_FORM_ABCDEFGHLONGLEE123 = "[FR]singleformABCDEFGHLONGLEE123.apkg";
        public const string FR_MULTIPLE_FORM_ABCDEFGHLONGLEE123 = "[FR]multiformABCDEFGHLONGLEE123.apkg";
        public const string VN_SINGLE_FORM_ABCDEFGHLONGLEE123 = "[VN]singleformABCDEFGHLONGLEE123.apkg";

        public const string ANKI_PNG = "anki.png";
    }

    public class DirectoryPath
    {
        public static string ANKI_FLASHCARDS = @".\AnkiFlashcards";
        public static string OXFORD = Path.Combine(ANKI_FLASHCARDS, "oxlayout");
        public static string SOHA = Path.Combine(ANKI_FLASHCARDS, "soha");
        public static string LACVIET = Path.Combine(ANKI_FLASHCARDS, "lacViet");
        public static string CAMBRIDGE = Path.Combine(ANKI_FLASHCARDS, "cambridge");
        public static string COLLINS = Path.Combine(ANKI_FLASHCARDS, "collins");
        public static string SOUND = Path.Combine(ANKI_FLASHCARDS, "sounds");
        public static string IMAGE = Path.Combine(ANKI_FLASHCARDS, "images");
    }

    public class Languages
    {
        public static string EN_SRC = "[EN]";
        public static string EN2EN = $"{EN_SRC} English";
        public static string EN2VI = $"{EN_SRC} Vietnamese";
        public static string EN2CH = $"{EN_SRC} Chinese";
        public static string EN2EN_VI = $"{EN_SRC} English & Vietnamese";
        public static string EN2VI_EN = $"{EN_SRC} Vietnamese & English";

        public static string FR_SRC = "[FR]";
        public static string FR2VI = $"{FR_SRC} Vietnamese";
        public static string FR2EN = $"{FR_SRC} English";
        public static string FR2EN_VI = $"{FR_SRC} English & Vietnamese";
        public static string FR2VI_EN = $"{FR_SRC} Vietnamese & English";

        public static string VN_SRC = "[VN]";
        public static string VN2EN = $"{VN_SRC} English";
        public static string VN2FR = $"{VN_SRC} French";
    }

    public struct Constant
    {
        public const string GENERATE_CARD_FOR_THE_WORD = @"Generate Card For The Word: ";
        public const string TAB = "\t";
        public const string CR = "\r";
        public const string LF = "\n";
        public const string MEANING = "<MEANING>";
    }

    public struct Dictionary
    {
        public const string OXFORD_DOMAIN = @"www.oxfordlearnersdictionaries.com";
        public const string OXFORD_SPELLING_WRONG_1 = @"Did you spell it correctly?";
        public const string OXFORD_SPELLING_WRONG_2 = @"Oxford Learner's Dictionaries | Find the meanings";
        public const string OXFORD_URL_EN_EN = @"http://www.oxfordlearnersdictionaries.com/search/english/direct/?q={0}";

        public const string LACVIET_DOMAIN = @"tratu.coviet.vn";
        public const string LACVIET_URL_VN_EN = @"http://tratu.coviet.vn/tu-dien-lac-viet.aspx?learn=hoc-tieng-anh&t=V-A&k={0}";
        public const string LACVIET_URL_VN_FR = @"http://tratu.coviet.vn/tu-dien-lac-viet.aspx?learn=hoc-tieng-phap&t=V-F&k={0}";
        public const string LACVIET_URL_EN_VN = @"http://tratu.coviet.vn/tu-dien-lac-viet.aspx?learn=hoc-tieng-anh&t=A-V&k={0}";
        public const string LACVIET_URL_FR_VN = @"http://tratu.coviet.vn/tu-dien-lac-viet.aspx?learn=hoc-tieng-phap&t=F-V&k={0}";
        public const string LACVIET_SPELLING_WRONG = @"Dữ liệu đang được cập nhật";

        public const string CAMBRIDGE_DOMAIN = @"dictionary.cambridge.org";
        public const string CAMBRIDGE_URL_EN_CN = @"http://dictionary.cambridge.org/zhs/%E6%90%9C%E7%B4%A2/english-chinese-simplified/direct/?q={0}";
        public const string CAMBRIDGE_SPELLING_WRONG = @"你拼写正确了吗？";

        public const string COLLINS_DOMAIN = @"www.collinsdictionary.com";
        public const string COLLINS_URL_FR_EN = @"https://www.collinsdictionary.com/search/?dictCode=french-english&q={0}";
        public const string COLLINS_SPELLING_WRONG = @"CollinsDictionary.com | Collins Dictionaries - Free Online";

        public const string COPYRIGHT_EN2EN = "The flashcard's content belongs to the Oxford Advanced Learner's Dictionary. Thanks Oxford Dictionary!";
        public const string COPYRIGHT_EN2VI = "The flashcard's content belongs to the Oxford Advanced Learner's & LacViet Online Dictionaries. Thanks Oxford & LacViet Dictionaries!";
        public const string COPYRIGHT_EN2CH = "The flashcard's content belongs to the Oxford Advanced Learner's & Cambridge Online Dictionaries. Thanks Oxford & Cambridge Dictionaries!";
        public const string COPYRIGHT_VI = "The flashcard's content belongs to the LacViet Online Dictionary. Thanks LacViet Dictionary!";

        public const string COPYRIGHT_FR = "This flashcard's content is get from the Collins Dictionary. Thanks Collins Dictionary!";
        public const string COPYRIGHT_FR2VI = "The flashcard's content belongs to the Collins & LacViet Online Dictionaries. Thanks Collins & LacViet Dictionaries!";

        public const string NO_EXAMPLE = "There is no example for this word.";
    }
}
