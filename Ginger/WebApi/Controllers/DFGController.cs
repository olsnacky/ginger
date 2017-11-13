using GingerParser;
using GingerParser.DFG;
using GingerParser.Scope;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200, http://gngr.io, http://www.gngr.io, http://noilly.github.io", headers: "*", methods: "*")]
    public class DFGController : ApiController
    {
        public IHttpActionResult Post([FromBody]string source, [FromUri]bool performClosure)
        {
            if (source == null)
            {
                return BadRequest();
            }

            Parser parser = new Parser(source);
            parser.parse();
            //CFGVisitor cfgv = new CFGVisitor(parser.ast);
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            DFGVisitor dfgv = new DFGVisitor(parser.ast);
            if (performClosure)
            {
                dfgv.dfg.performClosure();
            }
            //DDGVisitor ddgv = new DDGVisitor(parser.ast);
            DfgJsonConverter jsonv = new DfgJsonConverter(dfgv.dfg);


            return Ok(jsonv.graph);
        }
    }
}
