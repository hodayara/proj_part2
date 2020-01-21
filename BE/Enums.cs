using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum statusGusReq { פתוח, נסגר_על_ידי_האתר, נסגר_כי_פג_תוקף }
    public enum area { צפון, דרום, מרכז, ירושלים }
    public enum subArea { שרון, ערבה, עוטף_עזה, גליל_עליון, גולן, גליל_תחתון, שומרון, הרי_יהודה, אילת, גוש_דן, בנימין }
    public enum type { צימר, דירת_אירוח, חדר_במלון, מאהל }
    public enum isNecessary { הכרחי, אפשרי, לא_מעונין }
    public enum statusOrder { טרם_טופל, נשלח_מייל, נסגר_מחוסר_הענות_של_הלקוח, נסגר_בהיענות_של_לקוח }

}
