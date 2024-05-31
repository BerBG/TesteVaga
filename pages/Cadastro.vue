<template>
  <v-container fluid class="product-form-container">
    <v-row justify="center">
      <v-col class="form-wrapper">
        <v-card>
          <v-card-title class="card-title">Cadastro de Produto</v-card-title>
          <v-card-text>
            <v-form>
              <v-text-field
                label="Nome do Produto"
                v-model="produto.nome"
                required
                @input="validateField('nome')"
              ></v-text-field>

              <v-text-field
                label="Descrição do Produto"
                v-model="produto.descricao"
                required
                @input="validateField('descricao')"
              ></v-text-field>

              <v-text-field
                label="Quantidade"
                type="number"
                v-model="produto.quantidade"
                required
                @input="validateField('quantidade')"
              ></v-text-field>

              <v-text-field
                label="Valor"
                type="number"
                v-model="produto.valor"
                required
                @input="validateField('valor')"
              ></v-text-field>

              <v-select
                label="Categoria"
                v-model="produto.categoria"
                :items="categorias"
                required
                @input="validateField('categoria')"
              ></v-select>

              <v-btn color="primary" @click="salvarProduto">Salvar</v-btn>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      hasAttemptedSubmit: false,
      produto: {
        nome: "",
        quantidade: 0,
        valor: 0,
        categoria: null,
        descricao: "",
      },
      categorias: [
        { value: 'Eletrônicos', text: "Eletrônicos" },
        { value: 'Vestuário', text: "Vestuário" },
        { value: 'Alimentos', text: "Alimentos" },
      ],
      errorMessages: {
        nome: "",
        quantidade: "",
        valor: "",
        categoria: "",
        descricao: "",
      },
    };
  },
  methods: {
    validateField(field) {
      if (this.hasAttemptedSubmit) {
        if (!this.produto[field]) {
          this.errorMessages[field] = `Preencha o ${field}`;
        } else {
          this.errorMessages[field] = "";
        }
      }
    },
    async salvarProduto() {
      this.hasAttemptedSubmit = true;
      if (
        this.produto.nome &&
        this.produto.quantidade &&
        this.produto.valor &&
        this.produto.categoria &&
        this.produto.descricao
      ) {
        // Criar um objeto com os dados do produto
        const produtoData = {
          nome: this.produto.nome,
          quantidade: this.produto.quantidade,
          valor: this.produto.valor,
          categoria: this.produto.categoria,
          descricao: this.produto.descricao
        };

        // Configurar a requisição HTTP para o endpoint de salvamento de produtos
        const apiUrl = "https://localhost:7146/Produto"; // Substituir por URL da sua API
        const config = {
          headers: {
            "Content-Type": "application/json",
          },
        };

        try {
          // Enviar a requisição para salvar o produto e aguardar a resposta
          const response = await axios.post(apiUrl, produtoData, config);

          // Verifique se a resposta indica sucesso
          if (response.status === 200 || response.status === 201) {
            // Limpar o formulário após o sucesso
            this.produto = {
              nome: "",
              quantidade: 0,
              valor: 0,
              categoria: null,
              descricao: "",
            };

            console.log("Produto salvo com sucesso!");
          } else {
            console.error("Erro ao salvar produto:", response.statusText);
          }
        } catch (error) {
          console.error(
            "Erro ao salvar produto:",
            error.response.data || error.message
          );
        }
      } else {
        console.error("Preencha todos os campos obrigatórios");
      }
    },
  },
};
</script>

<style scoped>
.product-form-container {
  margin: 20px;
}

.form-wrapper {
  max-width: 500px;
  padding: 20px;
}

.card-title {
  font-size: 24px;
  font-weight: bold;
}

.error-message {
  color: red;
  font-size: 12px;
  margin-top: 5px;
}
</style>