using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{

	HtmlWeb browser = new HtmlWeb(); //Instanciando o Browser

	List<Produto> Produtos = new List<Produto>();
	public IEnumerable<Produto> ShowAll(string product)
	{

		NCR(product);
		QRA(product);

		

		return Produtos;
	}

    #region Indo pegar os produtos da NCR
    private void NCR(string prodT)
    {
		HtmlDocument page = browser.Load(@"https://www.ncrangola.com/loja/particulares/pt/pesquisa?controller=search&orderby=position&orderway=desc&search_query="+prodT+"&submit_search=");
		int count = 0;

		foreach (var item in page.DocumentNode.SelectNodes("//li[@class='produto']"))
		{
			Produto produto = new Produto();
			produto.titulo = item.SelectNodes("//h6")[count].InnerText;
			produto.description = item.SelectNodes("//h5")[count].InnerText;
			produto.preco = item.SelectNodes("//div[@class='produto_precos_in']/span[1]")[count].InnerText;
			
			produto.image_ = item.SelectNodes("//div/span/a/img")[count].GetAttributeValue("src", "Empity").ToString();
			produto.url = item.SelectNodes("//div/span/a")[count].GetAttributeValue("href", "none").ToString().Replace("+", "");
			produto.local = "NCR";

			Produtos.Add(produto);
			count++;
		}
	}
	#endregion
	private void QRA(string prodT)
	{
		HtmlDocument page = browser.Load(@"https://www.querapidoangola.com/?s="+prodT+"&post_type=product");
		
		int count = 0;

		foreach (var item in page.DocumentNode.SelectNodes("//div[@class='col-inner']"))
		{
			Produto produto = new Produto();
			produto.titulo = item.SelectNodes("//div[@class='title-wrapper']/p")[count].InnerText;
			produto.preco = item.SelectNodes("//span[@class='woocommerce-Price-amount amount']/bdi")[count].InnerText;

			produto.image_ = item.SelectNodes("//div[@class='image-none']/a/img")[count].GetAttributeValue("src", "Empity").ToString();
			produto.url = item.SelectNodes("//div[@class='title-wrapper']/p[2]/a")[count].GetAttributeValue("href", "none").ToString().Replace("+", "");
			produto.local = "Que Rápido Angola";
count++;
			Produtos.Add(produto);
			
		}
	}
}

