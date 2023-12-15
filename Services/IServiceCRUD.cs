using Analiza_Risc.Models;

namespace Analiza_Risc.Services;

public interface IServiceCRUD
{

    void InregisrCompanie(AddCompanie companie, AddActiveImobilizate activeImobilizate,
        AddActiveCirculante activeCirculante, AddDatorii addDatorii, AddCapitaluri addCapitaluri);
}

