namespace CS_Caixa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ato", "IdUsuario", "dbo.Ato");
            DropIndex("dbo.Ato", new[] { "IdUsuario" });
            DropPrimaryKey("dbo.CadCheque");
            CreateTable(
                "dbo.Atendimento",
                c => new
                    {
                        AtendimentoId = c.Int(nullable: false, identity: true),
                        Fila = c.Int(),
                        Senha = c.String(maxLength: 6),
                        TipoAtendimento = c.String(maxLength: 1),
                        Data = c.DateTime(),
                        HoraRetirada = c.String(maxLength: 8),
                        Status = c.String(maxLength: 15),
                        HoraAtendimento = c.String(maxLength: 8),
                        IdUsuario = c.Int(),
                        NomeAtendente = c.String(maxLength: 50),
                        HoraFinalizado = c.String(maxLength: 8),
                        OrdemChamada = c.String(maxLength: 8),
                    })
                .PrimaryKey(t => t.AtendimentoId);
            
            CreateTable(
                "dbo.ItensAtoNotas",
                c => new
                    {
                        Id_AtoNotas = c.Int(nullable: false, identity: true),
                        Id_Ato = c.Int(nullable: false),
                        Cont = c.Int(),
                        Protocolo = c.Int(),
                        Recibo = c.Int(),
                        TipoAto = c.String(maxLength: 40),
                        Natureza = c.String(nullable: false, maxLength: 80),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Pmcmv = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Mutua = c.Decimal(precision: 18, scale: 2),
                        Acoterj = c.Decimal(precision: 18, scale: 2),
                        Distribuicao = c.Decimal(precision: 18, scale: 2),
                        QuantDistrib = c.Int(),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id_AtoNotas)
                .ForeignKey("dbo.Ato", t => t.Id_Ato, cascadeDelete: true)
                .Index(t => t.Id_Ato);
            
            CreateTable(
                "dbo.ItensAtoRgi",
                c => new
                    {
                        Id_AtoRgi = c.Int(nullable: false, identity: true),
                        Id_Ato = c.Int(nullable: false),
                        Cont = c.Int(),
                        Protocolo = c.Int(),
                        Recibo = c.Int(),
                        TipoAto = c.String(maxLength: 40),
                        Natureza = c.String(nullable: false, maxLength: 80),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Pmcmv = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Mutua = c.Decimal(precision: 18, scale: 2),
                        Acoterj = c.Decimal(precision: 18, scale: 2),
                        Distribuicao = c.Decimal(precision: 18, scale: 2),
                        QuantDistrib = c.Int(),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id_AtoRgi)
                .ForeignKey("dbo.Ato", t => t.Id_Ato, cascadeDelete: true)
                .Index(t => t.Id_Ato);
            
            CreateTable(
                "dbo.ItensCustasNotas",
                c => new
                    {
                        Id_Custa = c.Int(nullable: false, identity: true),
                        Id_Ato = c.Int(nullable: false),
                        Id_AtoNotas = c.Int(),
                        Tabela = c.String(maxLength: 20),
                        Item = c.String(maxLength: 20),
                        SubItem = c.String(maxLength: 20),
                        Quantidade = c.String(maxLength: 10),
                        Complemento = c.String(maxLength: 50),
                        Excessao = c.String(maxLength: 50),
                        Valor = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Descricao = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id_Custa)
                .ForeignKey("dbo.Ato", t => t.Id_Ato, cascadeDelete: true)
                .Index(t => t.Id_Ato);
            
            CreateTable(
                "dbo.ItensCustasProtesto",
                c => new
                    {
                        Id_Custa = c.Int(nullable: false, identity: true),
                        Id_Ato = c.Int(nullable: false),
                        Tabela = c.String(maxLength: 20),
                        Item = c.String(maxLength: 20),
                        SubItem = c.String(maxLength: 20),
                        Quantidade = c.String(maxLength: 10),
                        Complemento = c.String(maxLength: 50),
                        Excessao = c.String(maxLength: 50),
                        Valor = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Descricao = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id_Custa)
                .ForeignKey("dbo.Ato", t => t.Id_Ato, cascadeDelete: true)
                .Index(t => t.Id_Ato);
            
            CreateTable(
                "dbo.ItensCustasRgi",
                c => new
                    {
                        Id_Custa = c.Int(nullable: false, identity: true),
                        Id_AtoRgi = c.Int(nullable: false),
                        Id_Ato = c.Int(nullable: false),
                        Cont = c.Int(),
                        Tabela = c.String(maxLength: 20),
                        Item = c.String(maxLength: 20),
                        SubItem = c.String(maxLength: 20),
                        Quantidade = c.String(maxLength: 10),
                        Complemento = c.String(maxLength: 50),
                        Excessao = c.String(maxLength: 50),
                        Valor = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Descricao = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id_Custa)
                .ForeignKey("dbo.Ato", t => t.Id_Ato, cascadeDelete: true)
                .Index(t => t.Id_Ato);
            
            CreateTable(
                "dbo.AtualizaSite",
                c => new
                    {
                        AtualizaSiteId = c.Int(nullable: false),
                        DataAtualizacao = c.String(maxLength: 10),
                        HoraAtualizacao = c.String(maxLength: 8),
                        Status = c.String(maxLength: 20),
                        PcAtualizacao = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.AtualizaSiteId);
            
            CreateTable(
                "dbo.CadastroCompraMaterial",
                c => new
                    {
                        IdCompraMaterial = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Quant = c.Int(nullable: false),
                        CadFunc = c.String(nullable: false, maxLength: 100),
                        NotaFiscal = c.String(maxLength: 50),
                        Descricao = c.String(maxLength: 100),
                        ValorUni = c.Single(),
                        ValorTotal = c.Single(),
                    })
                .PrimaryKey(t => new { t.IdCompraMaterial, t.Data, t.Quant, t.CadFunc });
            
            CreateTable(
                "dbo.CadastroMaterial",
                c => new
                    {
                        IdDescricao = c.Int(nullable: false, identity: true),
                        DescMaterial = c.String(nullable: false, maxLength: 100),
                        Quantidade = c.Int(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => new { t.IdDescricao, t.DescMaterial, t.Quantidade, t.Codigo });
            
            CreateTable(
                "dbo.ConexaoPainelSenhas",
                c => new
                    {
                        ConexaoId = c.Int(nullable: false, identity: true),
                        IpServidorAtendimento = c.String(nullable: false, maxLength: 15),
                        PortaConexao = c.Int(nullable: false),
                        IpServidorAtendimentoNotas = c.String(maxLength: 15),
                        PortaConexaoNotas = c.Int(),
                    })
                .PrimaryKey(t => t.ConexaoId);
            
            CreateTable(
                "dbo.ControleAtos",
                c => new
                    {
                        Id_ControleAtos = c.Int(nullable: false, identity: true),
                        DataAto = c.DateTime(nullable: false),
                        AtoNaoGratuito = c.Int(),
                        AtoGratuito = c.Int(),
                        IdUsuario = c.Int(nullable: false),
                        Usuario = c.String(maxLength: 100),
                        Atribuicao = c.String(maxLength: 15),
                        LetraSelo = c.String(maxLength: 4),
                        NumeroSelo = c.Int(),
                        Faixa = c.String(maxLength: 100),
                        Matricula = c.String(maxLength: 10),
                        Livro = c.String(maxLength: 10),
                        FolhaInical = c.Int(),
                        FolhaFinal = c.Int(),
                        NumeroAto = c.Int(),
                        Protocolo = c.Int(),
                        Recibo = c.Int(),
                        Id_Ato = c.Int(),
                        ReciboBalcao = c.Int(),
                        TipoAto = c.String(maxLength: 40),
                        Natureza = c.String(nullable: false, maxLength: 80),
                        Convenio = c.String(maxLength: 1, fixedLength: true),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Pmcmv = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Mutua = c.Decimal(precision: 18, scale: 2),
                        Acoterj = c.Decimal(precision: 18, scale: 2),
                        QtdAtos = c.Int(),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id_ControleAtos);
            
            CreateTable(
                "dbo.ItensControleAtoNotas",
                c => new
                    {
                        Id_ControleAtoNotas = c.Int(nullable: false, identity: true),
                        Id_ControleAto = c.Int(nullable: false),
                        Cont = c.Int(),
                        Protocolo = c.Int(),
                        Recibo = c.Int(),
                        TipoAto = c.String(maxLength: 40),
                        Natureza = c.String(nullable: false, maxLength: 80),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Pmcmv = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Mutua = c.Decimal(precision: 18, scale: 2),
                        Acoterj = c.Decimal(precision: 18, scale: 2),
                        Distribuicao = c.Decimal(precision: 18, scale: 2),
                        QuantDistrib = c.Int(),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id_ControleAtoNotas)
                .ForeignKey("dbo.ControleAtos", t => t.Id_ControleAto, cascadeDelete: true)
                .Index(t => t.Id_ControleAto);
            
            CreateTable(
                "dbo.ItensControleAtoProtesto",
                c => new
                    {
                        Id_ControleAtoProtesto = c.Int(nullable: false, identity: true),
                        Id_ControleAto = c.Int(nullable: false),
                        Cont = c.Int(),
                        Protocolo = c.Int(),
                        Recibo = c.Int(),
                        TipoAto = c.String(maxLength: 40),
                        Natureza = c.String(nullable: false, maxLength: 80),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Pmcmv = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Mutua = c.Decimal(precision: 18, scale: 2),
                        Acoterj = c.Decimal(precision: 18, scale: 2),
                        Distribuicao = c.Decimal(precision: 18, scale: 2),
                        QuantDistrib = c.Int(),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id_ControleAtoProtesto)
                .ForeignKey("dbo.ControleAtos", t => t.Id_ControleAto, cascadeDelete: true)
                .Index(t => t.Id_ControleAto);
            
            CreateTable(
                "dbo.ItensCustasControleAtosNotas",
                c => new
                    {
                        Id_Custa = c.Int(nullable: false, identity: true),
                        Id_ControleAto = c.Int(nullable: false),
                        Id_AtoNotas = c.Int(),
                        Tabela = c.String(maxLength: 20),
                        Item = c.String(maxLength: 20),
                        SubItem = c.String(maxLength: 20),
                        Quantidade = c.String(maxLength: 10),
                        Complemento = c.String(maxLength: 50),
                        Excessao = c.String(maxLength: 50),
                        Valor = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Descricao = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id_Custa)
                .ForeignKey("dbo.ControleAtos", t => t.Id_ControleAto, cascadeDelete: true)
                .Index(t => t.Id_ControleAto);
            
            CreateTable(
                "dbo.ItensCustasControleAtosProtesto",
                c => new
                    {
                        Id_Custa = c.Int(nullable: false, identity: true),
                        Id_ControleAto = c.Int(nullable: false),
                        Id_AtoProtesto = c.Int(),
                        Tabela = c.String(maxLength: 20),
                        Item = c.String(maxLength: 20),
                        SubItem = c.String(maxLength: 20),
                        Quantidade = c.String(maxLength: 10),
                        Complemento = c.String(maxLength: 50),
                        Excessao = c.String(maxLength: 50),
                        Valor = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Descricao = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id_Custa)
                .ForeignKey("dbo.ControleAtos", t => t.Id_ControleAto, cascadeDelete: true)
                .Index(t => t.Id_ControleAto);
            
            CreateTable(
                "dbo.ControlePagamentoCredito",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 100),
                        TipoCredito = c.String(maxLength: 50),
                        IdUsuario = c.Int(),
                        Usuario = c.String(maxLength: 100),
                        Origem = c.String(maxLength: 50),
                        Importado = c.Boolean(),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ControlePagamentoDebito",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Importado = c.Boolean(),
                        Descricao = c.String(nullable: false, maxLength: 100),
                        TipoDebito = c.String(maxLength: 50),
                        IdUsuario = c.Int(),
                        Usuario = c.String(maxLength: 100),
                        Origem = c.String(maxLength: 100),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustasDistribuicao",
                c => new
                    {
                        Id_custas = c.Int(nullable: false, identity: true),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Pmcmv = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Quant_Exced = c.Int(),
                        Total = c.Decimal(precision: 18, scale: 2),
                        VrFixo = c.Decimal(precision: 18, scale: 2),
                        VrExced = c.Decimal(precision: 18, scale: 2),
                        Ano = c.Int(),
                    })
                .PrimaryKey(t => t.Id_custas);
            
            CreateTable(
                "dbo.CustasNotas",
                c => new
                    {
                        Id_Custas = c.Int(nullable: false, identity: true),
                        ORDEM = c.Int(nullable: false),
                        ANO = c.Int(),
                        VAI = c.String(maxLength: 1, fixedLength: true),
                        TAB = c.String(maxLength: 20),
                        ITEM = c.String(maxLength: 20),
                        SUB = c.String(maxLength: 20),
                        DESCR = c.String(maxLength: 100),
                        VALOR = c.Decimal(precision: 18, scale: 2),
                        TEXTO = c.String(maxLength: 250),
                        TIPO = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id_Custas);
            
            CreateTable(
                "dbo.CustasProtesto",
                c => new
                    {
                        Id_Custas = c.Int(nullable: false, identity: true),
                        ORDEM = c.Int(nullable: false),
                        ANO = c.Int(),
                        VAI = c.String(maxLength: 1, fixedLength: true),
                        TAB = c.String(maxLength: 20),
                        ITEM = c.String(maxLength: 20),
                        SUB = c.String(maxLength: 20),
                        DESCR = c.String(maxLength: 100),
                        VALOR = c.Decimal(precision: 18, scale: 2),
                        TEXTO = c.String(maxLength: 250),
                        TIPO = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id_Custas);
            
            CreateTable(
                "dbo.CustasRgi",
                c => new
                    {
                        Id_Custas = c.Int(nullable: false, identity: true),
                        ORDEM = c.Int(nullable: false),
                        ANO = c.Int(),
                        VAI = c.String(maxLength: 1, fixedLength: true),
                        TAB = c.String(maxLength: 20),
                        ITEM = c.String(maxLength: 20),
                        SUB = c.String(maxLength: 20),
                        DESCR = c.String(maxLength: 100),
                        VALOR = c.Decimal(precision: 18, scale: 2),
                        TEXTO = c.String(maxLength: 250),
                        TIPO = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id_Custas);
            
            CreateTable(
                "dbo.ImportarMas",
                c => new
                    {
                        IdImportarMas = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(),
                        Atribuicao = c.String(maxLength: 50),
                        TipoAto = c.String(maxLength: 50),
                        Selo = c.String(maxLength: 9),
                        Aleatorio = c.String(maxLength: 3),
                        TipoCobranca = c.String(maxLength: 2),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Ressag = c.Decimal(precision: 18, scale: 2),
                        Mutua = c.Decimal(precision: 18, scale: 2),
                        Acoterj = c.Decimal(precision: 18, scale: 2),
                        Distribuidor = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdImportarMas);
            
            CreateTable(
                "dbo.IndiceEscritura",
                c => new
                    {
                        IdIndiceEscritura = c.Int(nullable: false, identity: true),
                        Outorgante = c.String(maxLength: 255),
                        Outorgado = c.String(maxLength: 255),
                        DiaDist = c.String(maxLength: 2),
                        MesDist = c.String(maxLength: 2),
                        AnoDist = c.String(maxLength: 4),
                        Natureza = c.String(maxLength: 255),
                        Dia = c.String(maxLength: 2),
                        Mes = c.String(maxLength: 2),
                        Ano = c.String(maxLength: 4),
                        Ato = c.String(maxLength: 5),
                        Livro = c.String(maxLength: 5),
                        Fls = c.String(maxLength: 5),
                        Ordem = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.IdIndiceEscritura);
            
            CreateTable(
                "dbo.IndiceProcuracao",
                c => new
                    {
                        IdIndiceProcuracao = c.Int(nullable: false, identity: true),
                        Outorgante = c.String(maxLength: 255),
                        Outorgado = c.String(maxLength: 255),
                        Dia = c.String(maxLength: 5),
                        Mes = c.String(maxLength: 5),
                        Ano = c.String(maxLength: 5),
                        Ato = c.String(maxLength: 10),
                        Livro = c.String(maxLength: 10),
                        Fls = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.IdIndiceProcuracao);
            
            CreateTable(
                "dbo.IndiceRegistros",
                c => new
                    {
                        IdIndiceRegistros = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 255),
                        Livro = c.String(maxLength: 255),
                        Reg = c.String(maxLength: 255),
                        Numero = c.String(maxLength: 255),
                        Ordem = c.String(maxLength: 255),
                        Fls = c.String(maxLength: 255),
                        TipoPessoa = c.String(maxLength: 1, fixedLength: true),
                        CpfCnpj = c.String(maxLength: 15),
                        TipoAto = c.String(maxLength: 100),
                        DataRegistro = c.DateTime(),
                        DataVenda = c.DateTime(),
                        Enviado = c.Boolean(),
                    })
                .PrimaryKey(t => t.IdIndiceRegistros);
            
            CreateTable(
                "dbo.Indisponibilidade",
                c => new
                    {
                        IdIndisponibilidade = c.Int(nullable: false, identity: true),
                        Titulo = c.String(maxLength: 100),
                        Nome = c.String(maxLength: 255),
                        CpfCnpj = c.String(maxLength: 255),
                        Oficio = c.String(maxLength: 255),
                        Aviso = c.String(maxLength: 255),
                        Processo = c.String(maxLength: 255),
                        Valor = c.String(),
                    })
                .PrimaryKey(t => t.IdIndisponibilidade);
            
            CreateTable(
                "dbo.Loteamentos",
                c => new
                    {
                        LoteamentoId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 200),
                        Proprietario = c.String(maxLength: 200),
                        Localizacao = c.String(maxLength: 15),
                        Matricula = c.Int(),
                        NumeroInscricao = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.LoteamentoId);
            
            CreateTable(
                "dbo.Portador",
                c => new
                    {
                        ID_PORTADOR = c.Int(nullable: false, identity: true),
                        CODIGO = c.String(maxLength: 10),
                        NOME = c.String(maxLength: 100),
                        TIPO = c.String(maxLength: 1, fixedLength: true),
                        DOCUMENTO = c.String(maxLength: 14),
                        ENDERECO = c.String(maxLength: 100),
                        BANCO = c.String(maxLength: 1, fixedLength: true),
                        CONVENIO = c.String(maxLength: 1, fixedLength: true),
                        CONTA = c.String(maxLength: 20),
                        OBSERVACAO = c.String(maxLength: 100),
                        AGENCIA = c.String(maxLength: 6),
                        PRACA = c.String(maxLength: 7),
                        CRA = c.String(maxLength: 1, fixedLength: true),
                        SEQUENCIA = c.Int(),
                        VALOR_DOC = c.Decimal(precision: 18, scale: 2),
                        VALOR_TED = c.Decimal(precision: 18, scale: 2),
                        NOMINAL = c.String(maxLength: 1, fixedLength: true),
                        FORCA_LEI = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.ID_PORTADOR);
            
            CreateTable(
                "dbo.ReciboBalcao",
                c => new
                    {
                        IdReciboBalcao = c.Int(nullable: false),
                        Data = c.DateTime(),
                        NumeroRecibo = c.Int(nullable: false),
                        IdUsuario = c.Int(),
                        Usuario = c.String(maxLength: 100),
                        Status = c.String(nullable: false, maxLength: 20),
                        Pago = c.Boolean(nullable: false),
                        TipoCustas = c.String(maxLength: 50),
                        TipoPagamento = c.String(maxLength: 50),
                        QuantAut = c.Int(),
                        QuantAbert = c.Int(),
                        QuantRecAut = c.Int(),
                        QuantRecSem = c.Int(),
                        QuantCopia = c.Int(),
                        ValorAdicionar = c.Decimal(precision: 18, scale: 2),
                        ValorDesconto = c.Decimal(precision: 18, scale: 2),
                        Mensalista = c.String(maxLength: 50),
                        NumeroRequisicao = c.Int(),
                        Emolumentos = c.Decimal(precision: 18, scale: 2),
                        Fetj = c.Decimal(precision: 18, scale: 2),
                        Fundperj = c.Decimal(precision: 18, scale: 2),
                        Funperj = c.Decimal(precision: 18, scale: 2),
                        Funarpen = c.Decimal(precision: 18, scale: 2),
                        Pmcmv = c.Decimal(precision: 18, scale: 2),
                        Iss = c.Decimal(precision: 18, scale: 2),
                        Mutua = c.Decimal(precision: 18, scale: 2),
                        Acoterj = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        ValorPago = c.Decimal(precision: 18, scale: 2),
                        ValorTroco = c.Decimal(precision: 18, scale: 2),
                        QuantMaterializacao = c.Int(),
                        IdAtendimento = c.Int(),
                    })
                .PrimaryKey(t => t.IdReciboBalcao);
            
            CreateTable(
                "dbo.RetiradaMaterial",
                c => new
                    {
                        IdRetiradaMaterial = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Quantidade = c.Int(nullable: false),
                        Material = c.String(nullable: false, maxLength: 100),
                        Funcionario = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.IdRetiradaMaterial, t.Data, t.Quantidade, t.Material, t.Funcionario });
            
            CreateTable(
                "dbo.SeloAtualBalcao",
                c => new
                    {
                        IdSeloBalcao = c.Int(nullable: false, identity: true),
                        Letra = c.String(maxLength: 4),
                        Numero = c.Int(),
                    })
                .PrimaryKey(t => t.IdSeloBalcao);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.ValorPago",
                c => new
                    {
                        ValorPagoId = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(),
                        IdAto = c.Int(nullable: false),
                        IdReciboBalcao = c.Int(nullable: false),
                        Dinheiro = c.Decimal(precision: 18, scale: 2),
                        Deposito = c.Decimal(precision: 18, scale: 2),
                        Cheque = c.Decimal(precision: 18, scale: 2),
                        ChequePre = c.Decimal(precision: 18, scale: 2),
                        Boleto = c.Decimal(precision: 18, scale: 2),
                        Mensalista = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ValorPagoId);
            
            CreateTable(
                "dbo.VerificaBackup",
                c => new
                    {
                        VerificaBackupId = c.Int(nullable: false, identity: true),
                        DataVerificacao = c.DateTime(),
                        HoraVerificacao = c.String(maxLength: 10),
                        Status = c.String(maxLength: 50),
                        MaquinaVerificou = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.VerificaBackupId);
            
            CreateTable(
                "dbo.VerificacaoCaixa",
                c => new
                    {
                        VerificacaoCaixaId = c.Int(nullable: false, identity: true),
                        Status = c.String(maxLength: 1),
                        Valor = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.VerificacaoCaixaId);
            
            AddColumn("dbo.Ato", "TipoPagamento", c => c.String(maxLength: 50));
            AddColumn("dbo.Ato", "Pago", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ato", "ValorEscrevente", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "ValorAdicionar", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "ValorDesconto", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "Mensalista", c => c.String(maxLength: 50));
            AddColumn("dbo.Ato", "ValorCorretor", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "Faixa", c => c.String(maxLength: 100));
            AddColumn("dbo.Ato", "Portador", c => c.String(maxLength: 100));
            AddColumn("dbo.Ato", "ValorTitulo", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "IdReciboBalcao", c => c.Int());
            AddColumn("dbo.Ato", "ReciboBalcao", c => c.Int());
            AddColumn("dbo.Ato", "TipoAto", c => c.String(maxLength: 40));
            AddColumn("dbo.Ato", "Funarpen", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "Iss", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "TipoPrenotacao", c => c.String(maxLength: 20));
            AddColumn("dbo.Ato", "QuantIndisp", c => c.Int());
            AddColumn("dbo.Ato", "QuantPrenotacao", c => c.Int());
            AddColumn("dbo.Ato", "QuantDistrib", c => c.Int());
            AddColumn("dbo.Ato", "QuantCopia", c => c.Int());
            AddColumn("dbo.Ato", "NumeroRequisicao", c => c.Int());
            AddColumn("dbo.Ato", "QtdAtos", c => c.Int());
            AddColumn("dbo.Ato", "ValorPago", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "ValorTroco", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "Aleatorio", c => c.String(maxLength: 3));
            AddColumn("dbo.Ato", "DescricaoAto", c => c.String(maxLength: 150));
            AddColumn("dbo.Ato", "FichaAto", c => c.String(maxLength: 15));
            AddColumn("dbo.Log", "Descricao", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Retirada_Caixa", "NumeroRecibo", c => c.Int());
            AddColumn("dbo.Usuarios", "ImprimirMatricula", c => c.Boolean());
            AlterColumn("dbo.Adicionar_Caixa", "Atribuicao", c => c.String(maxLength: 50));
            AlterColumn("dbo.Adicionar_Caixa", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.Adicionar_Caixa", "TpPagamento", c => c.String(maxLength: 50));
            AlterColumn("dbo.Ato", "Usuario", c => c.String(maxLength: 100));
            AlterColumn("dbo.Ato", "Atribuicao", c => c.String(maxLength: 15));
            AlterColumn("dbo.Ato", "LetraSelo", c => c.String(maxLength: 4));
            AlterColumn("dbo.Ato", "Livro", c => c.String(maxLength: 10));
            AlterColumn("dbo.Ato", "Recibo", c => c.Int());
            AlterColumn("dbo.Ato", "Natureza", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Ato", "Escrevente", c => c.String(maxLength: 100));
            AlterColumn("dbo.Ato", "Convenio", c => c.String(maxLength: 1, fixedLength: true));
            AlterColumn("dbo.Ato", "TipoCobranca", c => c.String(maxLength: 20));
            AlterColumn("dbo.CadCheque", "Valor", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CadCheque", "Caixa", c => c.String(maxLength: 15));
            AlterColumn("dbo.CadCheque", "NumCheque", c => c.String(maxLength: 20));
            AlterColumn("dbo.CadCheque", "Obs", c => c.String(maxLength: 100));
            AlterColumn("dbo.CadMensalista", "Nome", c => c.String(maxLength: 255));
            AlterColumn("dbo.Log", "Data", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Log", "Usuario", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Log", "Acao", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Log", "Tela", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Retirada_Caixa", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.Usuarios", "NomeUsu", c => c.String(maxLength: 50));
            AlterColumn("dbo.Usuarios", "Senha", c => c.String(maxLength: 50));
            AddPrimaryKey("dbo.CadCheque", new[] { "Id", "Data" });
            DropColumn("dbo.Ato", "Status");
            DropColumn("dbo.Ato", "Ar");
            DropColumn("dbo.Log", "Ato");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Log", "Ato", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AddColumn("dbo.Ato", "Ar", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ato", "Status", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ItensCustasControleAtosProtesto", "Id_ControleAto", "dbo.ControleAtos");
            DropForeignKey("dbo.ItensCustasControleAtosNotas", "Id_ControleAto", "dbo.ControleAtos");
            DropForeignKey("dbo.ItensControleAtoProtesto", "Id_ControleAto", "dbo.ControleAtos");
            DropForeignKey("dbo.ItensControleAtoNotas", "Id_ControleAto", "dbo.ControleAtos");
            DropForeignKey("dbo.ItensCustasRgi", "Id_Ato", "dbo.Ato");
            DropForeignKey("dbo.ItensCustasProtesto", "Id_Ato", "dbo.Ato");
            DropForeignKey("dbo.ItensCustasNotas", "Id_Ato", "dbo.Ato");
            DropForeignKey("dbo.ItensAtoRgi", "Id_Ato", "dbo.Ato");
            DropForeignKey("dbo.ItensAtoNotas", "Id_Ato", "dbo.Ato");
            DropIndex("dbo.ItensCustasControleAtosProtesto", new[] { "Id_ControleAto" });
            DropIndex("dbo.ItensCustasControleAtosNotas", new[] { "Id_ControleAto" });
            DropIndex("dbo.ItensControleAtoProtesto", new[] { "Id_ControleAto" });
            DropIndex("dbo.ItensControleAtoNotas", new[] { "Id_ControleAto" });
            DropIndex("dbo.ItensCustasRgi", new[] { "Id_Ato" });
            DropIndex("dbo.ItensCustasProtesto", new[] { "Id_Ato" });
            DropIndex("dbo.ItensCustasNotas", new[] { "Id_Ato" });
            DropIndex("dbo.ItensAtoRgi", new[] { "Id_Ato" });
            DropIndex("dbo.ItensAtoNotas", new[] { "Id_Ato" });
            DropPrimaryKey("dbo.CadCheque");
            AlterColumn("dbo.Usuarios", "Senha", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Usuarios", "NomeUsu", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Retirada_Caixa", "Descricao", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.Log", "Tela", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Log", "Acao", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Log", "Usuario", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Log", "Data", c => c.DateTime());
            AlterColumn("dbo.CadMensalista", "Nome", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.CadCheque", "Obs", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.CadCheque", "NumCheque", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.CadCheque", "Caixa", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.CadCheque", "Valor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Ato", "TipoCobranca", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("dbo.Ato", "Convenio", c => c.String(maxLength: 1, unicode: false));
            AlterColumn("dbo.Ato", "Escrevente", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Ato", "Natureza", c => c.String(nullable: false, maxLength: 80, unicode: false));
            AlterColumn("dbo.Ato", "Recibo", c => c.Int(nullable: false));
            AlterColumn("dbo.Ato", "Livro", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.Ato", "LetraSelo", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.Ato", "Atribuicao", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Ato", "Usuario", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Adicionar_Caixa", "TpPagamento", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Adicionar_Caixa", "Descricao", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.Adicionar_Caixa", "Atribuicao", c => c.String(maxLength: 50, unicode: false));
            DropColumn("dbo.Usuarios", "ImprimirMatricula");
            DropColumn("dbo.Retirada_Caixa", "NumeroRecibo");
            DropColumn("dbo.Log", "Descricao");
            DropColumn("dbo.Ato", "FichaAto");
            DropColumn("dbo.Ato", "DescricaoAto");
            DropColumn("dbo.Ato", "Aleatorio");
            DropColumn("dbo.Ato", "ValorTroco");
            DropColumn("dbo.Ato", "ValorPago");
            DropColumn("dbo.Ato", "QtdAtos");
            DropColumn("dbo.Ato", "NumeroRequisicao");
            DropColumn("dbo.Ato", "QuantCopia");
            DropColumn("dbo.Ato", "QuantDistrib");
            DropColumn("dbo.Ato", "QuantPrenotacao");
            DropColumn("dbo.Ato", "QuantIndisp");
            DropColumn("dbo.Ato", "TipoPrenotacao");
            DropColumn("dbo.Ato", "Iss");
            DropColumn("dbo.Ato", "Funarpen");
            DropColumn("dbo.Ato", "TipoAto");
            DropColumn("dbo.Ato", "ReciboBalcao");
            DropColumn("dbo.Ato", "IdReciboBalcao");
            DropColumn("dbo.Ato", "ValorTitulo");
            DropColumn("dbo.Ato", "Portador");
            DropColumn("dbo.Ato", "Faixa");
            DropColumn("dbo.Ato", "ValorCorretor");
            DropColumn("dbo.Ato", "Mensalista");
            DropColumn("dbo.Ato", "ValorDesconto");
            DropColumn("dbo.Ato", "ValorAdicionar");
            DropColumn("dbo.Ato", "ValorEscrevente");
            DropColumn("dbo.Ato", "Pago");
            DropColumn("dbo.Ato", "TipoPagamento");
            DropTable("dbo.VerificacaoCaixa");
            DropTable("dbo.VerificaBackup");
            DropTable("dbo.ValorPago");
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.SeloAtualBalcao");
            DropTable("dbo.RetiradaMaterial");
            DropTable("dbo.ReciboBalcao");
            DropTable("dbo.Portador");
            DropTable("dbo.Loteamentos");
            DropTable("dbo.Indisponibilidade");
            DropTable("dbo.IndiceRegistros");
            DropTable("dbo.IndiceProcuracao");
            DropTable("dbo.IndiceEscritura");
            DropTable("dbo.ImportarMas");
            DropTable("dbo.CustasRgi");
            DropTable("dbo.CustasProtesto");
            DropTable("dbo.CustasNotas");
            DropTable("dbo.CustasDistribuicao");
            DropTable("dbo.ControlePagamentoDebito");
            DropTable("dbo.ControlePagamentoCredito");
            DropTable("dbo.ItensCustasControleAtosProtesto");
            DropTable("dbo.ItensCustasControleAtosNotas");
            DropTable("dbo.ItensControleAtoProtesto");
            DropTable("dbo.ItensControleAtoNotas");
            DropTable("dbo.ControleAtos");
            DropTable("dbo.ConexaoPainelSenhas");
            DropTable("dbo.CadastroMaterial");
            DropTable("dbo.CadastroCompraMaterial");
            DropTable("dbo.AtualizaSite");
            DropTable("dbo.ItensCustasRgi");
            DropTable("dbo.ItensCustasProtesto");
            DropTable("dbo.ItensCustasNotas");
            DropTable("dbo.ItensAtoRgi");
            DropTable("dbo.ItensAtoNotas");
            DropTable("dbo.Atendimento");
            AddPrimaryKey("dbo.CadCheque", new[] { "Id", "Valor", "Data" });
            CreateIndex("dbo.Ato", "IdUsuario");
            AddForeignKey("dbo.Ato", "IdUsuario", "dbo.Ato", "Id_Ato");
        }
    }
}
