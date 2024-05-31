<template>
  <v-container class="produtos-container">
    <h2 class="title">Lista de Produtos</h2>
    <v-divider class="divider"></v-divider>
    <v-list>
      <v-list-item v-for="produto in produtos" :key="produto.id">
        <v-list-item-content>
          <v-list-item-title>{{ produto.nome }}</v-list-item-title>
          <v-list-item-subtitle>
            <div style="margin-bottom: 5px;">Quantidade: {{ produto.quantidade }}</div>
            <div style="margin-bottom: 5px;">Valor: R$ {{ produto.valor }}</div>
            <div>Descrição: {{ produto.descricao }}</div>
          </v-list-item-subtitle>
        </v-list-item-content>
        <v-list-item-action>
          <v-btn color="primary" @click="editarProduto(produto)" class="edit-btn">
            <v-icon>mdi-pencil</v-icon>
            <span>Editar</span>
          </v-btn>
          <v-btn color="error" @click="excluirProduto(produto)" class="delete-btn">
            <v-icon>mdi-delete</v-icon>
            <span>Excluir</span>
          </v-btn>
        </v-list-item-action>
      </v-list-item>
    </v-list>
    <v-btn color="primary" @click="carregarProdutos" class="load-button"
      >Recarregar Produtos</v-btn
    >
<!-- Formulário de Edição -->
    <v-dialog v-model="editando" max-width="500">
      <template v-slot:activator="{ on }"></template>
      <v-card>
        <v-card-title>Editar Produto</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="salvarEdicao">
            <v-text-field v-model="produtoEditando.nome" label="Nome do Produto"></v-text-field>
            <v-text-field v-model="produtoEditando.descricao" label="Descrição do Produto"></v-text-field>
            <v-text-field v-model="produtoEditando.quantidade" type="number" label="Quantidade"></v-text-field>
            <v-text-field v-model="produtoEditando.valor" type="number" label="Valor"></v-text-field>
            <v-select v-model="produtoEditando.categoria" :items="categorias" label="Categoria"></v-select>
            <v-btn type="submit" color="primary">Salvar</v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      produtos: [],
      editando: false, // Estado de edição
      produtoEditando: { // Produto em edição
        id: null,
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
    };
  },
  mounted() {
    this.carregarProdutos();
  },
  methods: {
    async carregarProdutos() {
      try {
        const response = await axios.get("https://localhost:7146/Produto");
        this.produtos = response.data;
      } catch (error) {
        console.error("Erro ao carregar produtos:", error.message);
      }
    },
    async editarProduto(produto) {
      this.editando = true;
      // Preencha os campos do formulário com os dados do produto selecionado
      this.produtoEditando = {
        id: produto.id,
        nome: produto.nome,
        quantidade: produto.quantidade,
        valor: produto.valor,
        categoria: produto.categoria,
        descricao: produto.descricao,
      };
    },
    async salvarEdicao() {
      const apiUrl = `https://localhost:7146/Produto/${this.produtoEditando.id}`;
      const config = {
        headers: {
          "Content-Type": "application/json",
        },
      };

      try {
        const response = await axios.put(apiUrl, this.produtoEditando, config);
        if (response.status === 200 || response.status === 204) {
          // Atualizar a lista de produtos com os dados atualizados
          this.carregarProdutos();
          // Definir o estado de edição como falso para fechar o formulário de edição
          this.editando = false;
          console.log("Produto editado com sucesso!");
        } else {
          console.error("Erro ao editar produto:", response.statusText);
        }
      } catch (error) {
        console.error("Erro ao editar produto:", error.message);
      }
    },
    async excluirProduto(produto) {
      try {
        const response = await axios.delete(
          `https://localhost:7146/Produto/${produto.id}`
        );
        if (response.status === 200 || response.status === 204) {
          this.produtos = this.produtos.filter((p) => p.id !== produto.id);
          console.log("Produto excluído com sucesso!");
        } else {
          console.error("Erro ao excluir produto:", response.statusText);
        }
      } catch (error) {
        console.error("Erro ao excluir produto:", error.message);
      }
    },
  },
};
</script>

<style scoped>
.produtos-container {
  max-width: 800px;
  margin: auto;
  padding: 20px;
}

.title {
  font-size: 24px;
  font-weight: bold;
  margin-bottom: 10px;
}

.divider {
  margin: 10px 0;
}

.load-button {
  margin-top: 20px;
}

.edit-btn,
.delete-btn {
  min-width: 100px;
  text-transform: none;
  margin: 5px;
}

.edit-btn {
  background-color: #1976d2; /* Cor de fundo do botão de editar */
}

.delete-btn {
  background-color: #f44336; /* Cor de fundo do botão de excluir */
}

.edit-btn span,
.delete-btn span {
  margin-left: 5px; /* Espaçamento entre o ícone e o texto */
}

/* Estilo adicional para os botões */
.edit-btn,
.delete-btn {
  align-self: flex-start; /* Alinha os botões à esquerda */
}